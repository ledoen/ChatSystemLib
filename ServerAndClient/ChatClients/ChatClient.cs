using CommunicationLib.CommunicationClients;
using ServerAndClient.Libraries;
using ServerAndClient.Models;

namespace ServerAndClient.ChatClients
{
    public class ChatClient : IChatClient
    {
        private readonly ICommunicationClient _communicationClient;
        private readonly List<ChatMessage> _historyMessageList = new();
        private string _userName;

        public ChatClient(ICommunicationClient client)
        {
            _communicationClient = client;
            AddEventHandlerToClient();
        }

        public event EventHandler ForcedOffline;

        public event EventHandler ReceivedMessage;

        public List<ChatMessage> HistoryMessageList => _historyMessageList;

        public string UserName { get => _userName; set => _userName = value; }

        public void Offline()
        {
            _communicationClient.DisConnectToServer();
        }

        public bool Online()
        {
            // 【1】连接到服务器
            var res = _communicationClient.ConnectToServer();
            if (res == false) return false;
            // 【2】发送上线消息
            InformThatOnline();
            return true;
        }

        public void SendMessage(string content)
        {
            SendMessage(new ChatMessage(_userName, content));
        }

        private void AddEventHandlerToClient()
        {
            _communicationClient.ConnectionDisconnected += OnConnectionDisconnected;
            _communicationClient.ReceivedDataFromServer += OnReceivedMessage;
        }

        private void InformThatOnline()
        {
            // 【1】生成消息
            var message = new ChatMessage(_userName, $"{_userName} is online");

            // 【2】发送消息
            SendMessage(message);
        }

        private void OnConnectionDisconnected(object? sender, EventArgs e)
        {
            ForcedOffline?.Invoke(this, new EventArgs());
        }

        private void OnReceivedMessage(object? sender, string e)
        {
            // 【1】解析消息
            var message = e.FromJson<ChatMessage>();
            if (message == null) return;

            // 【2】将消息加入列表
            _historyMessageList.Add(message);
            ReceivedMessage?.Invoke(sender, new EventArgs());
        }

        private void SendMessage(ChatMessage message)
        {
            // 【1】转换消息
            var messageStr = message.ToJSON();
            // 【2】发送消息
            _communicationClient.SendDataToServer(messageStr);
        }
    }
}