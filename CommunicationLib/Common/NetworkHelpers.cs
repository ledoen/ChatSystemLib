using System.Net;
using System.Net.Sockets;

namespace CommunicationLib.Common
{
    public static class NetworkHelpers
    {

        public static IPAddress? GetLocalIPAddress()
        {
            var entry = Dns.GetHostEntry(Dns.GetHostName());
            var localAddress = entry.AddressList.
                FirstOrDefault(address => address.AddressFamily == AddressFamily.InterNetwork);
            return localAddress;
        }
    }
}