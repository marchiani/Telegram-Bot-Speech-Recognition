using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TaraBot.Service.Interface;
using TaraBot.Service.Settings;
using TataBot.Common.Helpers;
using TataBot.Common.Models.Zamzar;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace TaraBot.Service.Implementations
{
    public class VoiceTranscriberService : IVoiceTranscriberService
    {
        private readonly AzureSpeechToTextSettign AzureSpeechToText;
        private readonly ZamzarSettings ZamzarSettings;

        private readonly TelegramBotClient TelegramBotClient;

        public VoiceTranscriberService(
            AzureSpeechToTextSettign azureSpeechToText,
            TelegramBotSettings telegramBotSettings,
            ZamzarSettings zamzarSettings
            )
        {
            AzureSpeechToText = azureSpeechToText;
            ZamzarSettings = zamzarSettings;

            TelegramBotClient = new TelegramBotClient(telegramBotSettings.Token);
        }

        public async Task SpeechToText(Message message)
        {
            string fileId = message.Voice == null ? message.Audio.FileId : message.Voice.FileId;
            var file = await TelegramBotClient.GetFileAsync(fileId);
            string pathToAudio = GetFilePath(file.FileUniqueId, file.FilePath);
            var saveFileStream = File.Open(pathToAudio, FileMode.Create);

            TelegramBotClient.DownloadFileAsync(file.FilePath, saveFileStream).Wait();
            saveFileStream.Close();

            string fileType = message.Type == MessageType.Audio ? "file" : "voice";
            await TelegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"We started processing your {fileType} wait couple minute"
            );

            if (message.Type == MessageType.Voice && File.Exists(pathToAudio) && !File.Exists(Path.ChangeExtension(pathToAudio, ".ogg")))
            {
                File.Move(pathToAudio, Path.ChangeExtension(pathToAudio, ".ogg"));
                pathToAudio = Path.ChangeExtension(pathToAudio, ".ogg");
            }

            string jobEndpoint = ZamzarSettings.BaseAPIUrl + "jobs";

            var zamzarJob = ZamzarHelper.Upload<ZamzarJobResponseModel>(ZamzarSettings.SecretKey, jobEndpoint, pathToAudio, "wav").Result;

            string getStatusConvertingEndpoint = $"{jobEndpoint}/{zamzarJob.Id}";

            while (true)
            {
                zamzarJob = ZamzarHelper.GetSimpleResponse<ZamzarJobResponseModel>(ZamzarSettings.SecretKey, getStatusConvertingEndpoint).Result;
                if (zamzarJob.Status == "successful")
                {
                    break;
                }
            }

            string downloadConvertedFileEndpoint = $"{ZamzarSettings.BaseAPIUrl}files/{zamzarJob.TargetFiles.First().Id}/content";
            pathToAudio = Path.ChangeExtension(pathToAudio, ".wav");

            await ZamzarHelper.Download(ZamzarSettings.SecretKey, downloadConvertedFileEndpoint, pathToAudio);

            var config = SpeechConfig.FromSubscription(AzureSpeechToText.SubscriptionKey, AzureSpeechToText.Region);
            var autoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(ZamzarSettings.Languages);

            using var audioInput = AudioConfig.FromWavFileInput(pathToAudio);
            using var recognizer = new SpeechRecognizer(config, autoDetectSourceLanguageConfig, audioInput);

            var speechRecognitionResult = await recognizer.RecognizeOnceAsync();

            string result = null;

            switch (speechRecognitionResult.Reason)
            {
                case ResultReason.RecognizedSpeech:
                    result = speechRecognitionResult.Text;
                    break;
                case ResultReason.NoMatch:
                    result = "Speech could not be recognized";
                    break;
                case ResultReason.Canceled:
                    var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                    result = cancellation.ErrorDetails;
                    break;

            }

            await TelegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: result
            );
        }

        #region Helpers
        private static string GetFilePath(string fileUniqueId, string filePath1)
        {
            string fileName = $"{fileUniqueId}.{filePath1.Split('.').Last()}";
            string filePath = Path.Combine(Path.GetTempPath(), fileName);
            int count = 1;

            while (true)
            {
                if (File.Exists(filePath) || File.Exists(filePath.Replace(".oga", ".ogg")))
                {
                    if (count == 1)
                    {
                        string oldFileName = filePath.Split("\\").Last().Split('.').First();
                        string newFileName = oldFileName + $"_{count}";
                        filePath = filePath.Replace($"{oldFileName}", newFileName);
                    }
                    else
                    {
                        filePath = filePath.Replace($"_{count - 1}", $"_{count}");
                    }
                    count++;
                }
                else
                {
                    return filePath;
                }
            }
        }
        #endregion
    }
}