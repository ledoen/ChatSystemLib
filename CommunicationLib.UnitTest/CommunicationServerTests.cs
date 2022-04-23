using CommunicationLib.Common;
using CommunicationLib.CommunictionServers;
using NUnit.Framework;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CommunicationLib.UnitTest
{
    internal class CommunicationServerTests
    {
        private CommunictionServer? _server;

        [SetUp]
        public void Setup()
        {
            _server = new CommunictionServers.CommunictionServer();
        }
        [Test]
        public void StartListening_InputWithUnusedPort_Pass()
        {
            Task.Run(() => _server.StartListening(11000));
            Assert.Pass();
        }

        [Test]
        public void StartListening_InputWithUsedPort_ThrowException()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(NetworkHelpers.GetLocalIPAddress(), 10000));
            Assert.That(() => _server.StartListening(10000), Throws.Exception);
            socket.Close();
        }
    }
}
