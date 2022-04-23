using CommunicationLib.CommunictionServers;
using ServerAndClient.Libraries;
using ServerAndClient.Models;

namespace ServerAndClient.ChatServers
{
    public class ChatServer : IChatServer
    {
        private readonly ICommunicationServer _communicationServer;
        private readonly Dictionary<int, string> userChannelPairs = new();

        public ChatServer(ICommunicationServer communicationServer)
        {
            _communicationServer = communicationServer;
            InitCommunicationServer();
        }

        public void StartServer(int port)
        {
            try
            {
                _communicationServer.StartListening(port);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Network initialized error, server stoped, info: " + ex.ToString());
            }
        }

        private void AddUserToChannelPairs(int channelID, string userName)
        {
            if (userChannelPairs.ContainsKey(channelID) == false)
            {
                // 【3.1】如果不在，则添加到列表
                userChannelPairs[channelID] = userName;
            }
            // 【3.2】如果在，不处理
        }

        private void InformThatUserOffline(int e)
        {
            if (userChannelPairs.ContainsKey(e) == false) return;

            // 【1】生成消息
            var message = new ChatMessage("系统消息", $"{userChannelPairs[e]} is offline.");

            // 【2】序列化消息
            var messageStr = message.ToJSON();

            // 【3】发送消息
            _communicationServer.TailAndSendDataToClients(messageStr);
        }

        private void InitCommunicationServer()
        {
            _communicationServer.ClientConnected += OnClientConnected;
            _communicationServer.ClientDisconnected += OnClientDisconnected;
            _communicationServer.ReceivedDataFromClient += OnReceivedDataFromClient;
        }

        private void OnClientConnected(object? sender, int e)
        {
            Console.WriteLine($"client {e} is connedted");
        }

        private void OnClientDisconnected(object? sender, int e)
        {
            Console.WriteLine($"client {e} is disconnected");

            // 【1】构造发送消息通知其他用户
            InformThatUserOffline(e);

            // 【2】将用户从列表删除
            userChannelPairs.Remove(e);
        }

        private void OnReceivedDataFromClient(object? sender, (int, string) e)
        {
            if (string.IsNullOrEmpty(e.Item2)) return;
            // 【0】转发消息
            _communicationServer.SendDataToClients(e.Item2);
            // 【1】提取信息
            ChatMessage message = ChatMessageHelpers.ResolveMessageFromReceivedData(e.Item2);
            if (message == null) return;
            // 【2】添加用户到列表
            AddUserToChannelPairs(e.Item1, message.UserName);
        }
    }
}