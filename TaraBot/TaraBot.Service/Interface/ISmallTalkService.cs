namespace TaraBot.Service.Interface
{
    public interface ISmallTalkService
    {
        void SpeakWithBot(string textMessage, long chatId);
    }
}
