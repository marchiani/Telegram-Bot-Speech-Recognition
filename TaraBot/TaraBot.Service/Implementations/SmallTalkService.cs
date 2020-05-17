using ApiAiSDK;
using TaraBot.Service.Interface;
using TaraBot.Service.Settings;
using Telegram.Bot;

namespace TaraBot.Service.Implementations
{
    public class SmallTalkService : ISmallTalkService
    {
        private readonly ApiAi ApiAi;

        private readonly TelegramBotClient TelegramBotClient;
        public SmallTalkService(SmallTalkSettings smallTalkSettings, TelegramBotSettings telegramBotSettings)
        {
            var config = new AIConfiguration(smallTalkSettings.Token, SupportedLanguage.Russian);

            ApiAi = new ApiAi(config);

            TelegramBotClient = new TelegramBotClient(telegramBotSettings.Token);
        }

        public async void SpeakWithBot(string textMessage, long chatId)
        {
            var responseFromApi = ApiAi.TextRequest(textMessage);
            string result = responseFromApi.Result.Fulfillment.Speech;

            if (result == "")
            {
                result = "Простите, я не понял, повторите еще раз, пожалуйста.";
            }

            await TelegramBotClient.SendTextMessageAsync(
                chatId: chatId,
                text: result
            );
        }
    }
}
