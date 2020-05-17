using Microsoft.Extensions.DependencyInjection;
using TaraBot.Service.Settings;
using Telegram.Bot;

namespace TaraBot.Extention
{
    public static class SetBotWebhook
    {
        public static IServiceCollection AddTelegramBotClient
        (
            this IServiceCollection serviceCollection,
            TelegramBotSettings telegramBotConfiguration
        )
        {
            var client = new TelegramBotClient(telegramBotConfiguration.Token);
            string webHook = $"{telegramBotConfiguration.Url}/api/message/update";
            client.SetWebhookAsync(webHook).Wait();

            return serviceCollection
                .AddTransient<ITelegramBotClient>(x => client);
        }
    }
}