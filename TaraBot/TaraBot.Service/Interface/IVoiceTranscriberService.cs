using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TaraBot.Service.Interface
{
    public interface IVoiceTranscriberService
    {
        Task SpeechToText(Message message);
    }
}