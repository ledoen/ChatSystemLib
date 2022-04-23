using System.Net.Sockets;
using System.Text;

namespace CommunicationLib.Common
{
    internal class StateObject
    {
        public StateObject(Socket worker)
        {
            Worker = worker;
        }

        public StateObject()
        { }

        public static int BufferSize { get; set; } = 1024;
        public byte[] Buffer { get; set; } = new byte[BufferSize];
        public StringBuilder ReceivedSb { get; set; } = new StringBuilder();
        public Socket Worker { get; set; }
    }
}