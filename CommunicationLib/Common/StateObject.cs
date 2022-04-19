using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationLib.Common
{
    internal class StateObject
    {
        public static int BufferSize { get; set; } = 1024;
        public byte[] Buffer { get; set; } = new byte[BufferSize];
        public StringBuilder ReceivedSb { get; set; } = new StringBuilder();
        public Socket Worker { get; set; }
    }
}
