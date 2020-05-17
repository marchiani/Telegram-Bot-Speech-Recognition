using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using TaraBot.Service.Settings;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace TaraBot.Commands
{
    public class SpeechToTextCommandBuilder
    {
        private readonly AzureSpeechToTextSettign AzureSpeechToText;

        public SpeechToTextCommandBuilder(AzureSpeechToTextSettign azureSpeechToText)
        {
            AzureSpeechToText = azureSpeechToText;
        }

        public async Task<string> RecognizeSpeechAsync(string file)
        {
            var result = "";
            var filePath = Path.Combine(@"c:\Users\dimab\Desktop\TraraBot\tarabot\TaraBot\", file); 
            File.Move(filePath, Path.ChangeExtension(filePath, ".wav"));

            var config = SpeechConfig.FromSubscription(AzureSpeechToText.SubscriptionKey, AzureSpeechToText.Region);

            using (var recognizer = new SpeechRecognizer(config, filePath))
            {
                var resultSpeechToText = await recognizer.RecognizeOnceAsync();

                if (resultSpeechToText.Reason == ResultReason.RecognizedSpeech)
                {
                    result = resultSpeechToText.Text;
                }
            }

            return result;
        }
    }
}