using System.Linq;
using System.Threading.Tasks;
using TaraBot.Service.Interface;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TaraBot.Commands
{
    public class DistributorCommandBuilder : BaseCommandBuilder
    {
        private readonly TelegramBotClient TelegramBotClient;

        private readonly IVoiceTranscriberService VoiceTranscriberService;

        private readonly ISmallTalkService SmallTalkService;

        private readonly Update Update;

        public DistributorCommandBuilder
        (
            TelegramBotClient telegramBotClient,
            IVoiceTranscriberService voiceTranscriberService,
            ISmallTalkService smallTalkService,

            Update update
        )
        {
            TelegramBotClient = telegramBotClient;

            VoiceTranscriberService = voiceTranscriberService;

            SmallTalkService = smallTalkService;

            Update = update;
        }

        public async Task DefineCommand()
        {
            var message = Update.Message;

            switch (message.Type)
            {
                case MessageType.Voice:
                    await VoiceTranscriberService.SpeechToText(message);
                    break;
                case MessageType.Audio:
                    await VoiceTranscriberService.SpeechToText(message);
                    break;
                case MessageType.Text:
                    await TextMessages(message);
                    break;
                default:
                    break;
            }
        }

        public async Task TextMessages(Message message)
        {
            switch (message?.Text.Split(' ').First().Trim('/', ' ').ToLower())
            {
                case "menu":
                    string menuText =
@"Menu of TaraBot:
/menu - посмотреть меню
/keyboard - действия
/inline - полезные ссылки
/about - узнать больше обо мне";

                    await TelegramBotClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: menuText
                    );
                    
                    break;
                case "keyboard":
                    var infoKeyboard = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("Связаться с нами!") {RequestContact = true}
                        },
                        new[]
                        {
                            new KeyboardButton("Где Я?") {RequestLocation = true}
                        }
                    });

                    await TelegramBotClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Действия для пользователя!",
                        replyMarkup: infoKeyboard
                    );

                    break;
                case "inline":
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("YouTube", "https://www.youtube.com/")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("9GAG", "https://9gag.com/")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Our Project", "https://github.com/marchiani/Telegram-Bot-Speech-Recognition")
                        }
                    });

                    await TelegramBotClient.SendTextMessageAsync(
                        chatId: message.From.Id, 
                        text: "Полезные ссылки!",
                        replyMarkup: inlineKeyboard);

                    break;
                case "about":

                    var aboutKeyboard = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("Кто ты?")
                        },
                        new[]
                        {
                            new KeyboardButton("Что ты можешь?")
                        },
                        new[]
                        {
                            new KeyboardButton("Расскажи про своё хобби")
                        }
                    });

                    await TelegramBotClient.SendTextMessageAsync(
                        chatId: message.From.Id,
                        text: "Хочешь узнать обо мне?",
                        replyMarkup: aboutKeyboard);

                    break;
                default:
                    if (message.Text == "/start")
                    {
                        await TelegramBotClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Бип-Буп, Я загрузился и готов к работе!\n" +
                            "Нажмите на слово /menu " + "для просмотра меню!"
                            );
                    }
                    else
                    {
                        SmallTalkService.SpeakWithBot(message.Text, message.Chat.Id);
                    }
                    /// TODO: After edit menu add it opportunity to response message
                    break;
            }
        }
    }
}