using CommunicationLib.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationLib.UnitTest
{
    internal class NetworkHelpersTests
    {
        [Test]
        public void GetLocalIPAddress_WhenCalled_ReturnIPv4Address()
        { 
            var address = NetworkHelpers.GetLocalIPAddress();
            Assert.That(address, Is.Not.Null);
            Assert.That(address.AddressFamily, Is.EqualTo(AddressFamily.InterNetwork));
        }
    }
}
