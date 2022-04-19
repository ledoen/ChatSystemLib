using ServerAndClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAndClient.ChatClients
{
    public interface IChatClient
    {
        void Online();
        void Offline();
        void SendMessage(ChatMessage message);
        void OnReceivedMessage(object sender, string e);
    }
}
