using ServerAndClient.Models;

namespace ServerAndClient.ChatClients
{
    public interface IChatClient
    {
        List<ChatMessage> HistoryMessageList { get; }

        string UserName { get; set; }

        void Offline();

        bool Online();

        void SendMessage(string content);
    }
}