using CommunicationLib.CommunictionServers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAndClient.ChatServers
{
    public interface IChatServer
    {
    }

    public class ChatServer : IChatServer
    {
        private readonly ICommunicationServer _communicationServer;

        public ChatServer(ICommunicationServer communicationServer)
        {
            _communicationServer = communicationServer;
            _communicationServer.ClientConnected += OnClientConnected;
            _communicationServer.ClientDisconnected += OnClientDisconnected;
            _communicationServer.ReceivedDataFromClient += OnReceivedDataFromClient;

            _communicationServer.StartListening(11000);
        }

        private void OnReceivedDataFromClient(object? sender, (int,string) e)
        {
            _communicationServer.SendDataToClients(e.Item2);
        }

        private void OnClientDisconnected(object? sender, int e)
        {
            Console.WriteLine($"client {e} is disconnected");
        }

        private void OnClientConnected(object? sender, int e)
        {
            Console.WriteLine($"client {e} is connedted");
        }
    }
}
