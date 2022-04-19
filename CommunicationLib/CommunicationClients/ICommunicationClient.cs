using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationLib.CommunicationClients
{
    public interface ICommunicationClient
    {
        void ConnectToServer();
        event EventHandler<string> ReceivedDataFromServer;
        void SendDataToServer(string data);
        void DisConnectToServer();
        event EventHandler ConnectionDisconnected;
    }
}
