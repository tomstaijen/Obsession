using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class DenonTest
    {
        public string IpAddress { get; set; }

        [SetUp]
        public void Init()
        {
            IpAddress = "192.168.3.186";
        }

        public void CanConnect()
        {
            try
            {
                var socket = new TcpClient();
                socket.Connect(IpAddress, 23);
            }
            catch (Exception e)
            {
                Console.WriteLine("Oops");
            }
        }


        public void CanGetInfoFromDenon()
        {

        }
    }
}
