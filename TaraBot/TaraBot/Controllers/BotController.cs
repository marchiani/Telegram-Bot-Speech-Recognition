using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaraBot.Commands;
using TaraBot.Service.Interface;
using TaraBot.Service.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaraBot.Controllers
{
    [ApiController]
    [Route("api/message/update")]
    public class BotController : Controller
    {
        private readonly TelegramBotSettings TelegramBotConfiguration;

        private readonly IVoiceTranscriberService VoiceTranscriberService;

        private readonly ISmallTalkService SmallTalkService;

        private readonly TelegramBotClient TelegramBotClient;

        public BotController(TelegramBotSettings telegramBotConfiguration, IVoiceTranscriberService voiceTranscriberService, ISmallTalkService smallTalkService)
        {
            TelegramBotConfiguration = telegramBotConfiguration;
            VoiceTranscriberService = voiceTranscriberService;
            SmallTalkService = smallTalkService;

            TelegramBotClient = new TelegramBotClient(TelegramBotConfiguration.Token);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Started");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Update update)
        {
            if (update == null)
            {
                return Ok();
            }

            await new DistributorCommandBuilder(TelegramBotClient, VoiceTranscriberService, SmallTalkService, update).DefineCommand();

            return Ok();
        }
    }
}
