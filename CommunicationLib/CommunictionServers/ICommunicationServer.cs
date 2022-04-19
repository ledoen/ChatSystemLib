using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationLib.CommunictionServers
{
    public interface ICommunicationServer
    {
        // 参数为用户网络信息
        event EventHandler<int> ClientConnected;
        // 参数为用户网络信息
        event EventHandler<int> ClientDisconnected;
        // 参数1为用户网络信息，参数2为接收到的数据
        event EventHandler<(int, string)> ReceivedDataFromClient;
        void SendDataToClients(string data);
        void SendTailedDataToClients(string data);
        void StartListening(int port);
    }
}
