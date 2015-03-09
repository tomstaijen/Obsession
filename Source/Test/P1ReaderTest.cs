using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using P1Reader;

namespace Test
{
    [TestFixture]
    public class P1ReaderTest
    {
        [Test]
        public void Test()
        {
            var client = new TcpClient();
            client.Connect("192.168.3.29", 8899);

            var reader = new BinaryReader(client.GetStream());
            reader.ReadP1(p1 =>
                {
                    Console.WriteLine(p1.Gass);
                });

        }
    }
}
