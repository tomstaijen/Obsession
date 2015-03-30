using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Denon;
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

        [Test]
        public void CanGetInfoFromDenon()
        {
            var sut = new DenonConnection();
            sut.Connect("192.168.3.186");
            Console.WriteLine("Started");
//            Thread.Sleep(10000);
//            sut.Write("PWSTANDBY");
            sut.Write("SI?");
//            sut.Write("SV?");
//            sut.Write("NSA0Now Playing USB_????");
//            Thread.Sleep(2000);
//            sut.Write("PWON");
//            Thread.Sleep(2000);
            sut.Write("PW?");
//            Thread.Sleep(2000);
//            sut.Write("MV700");
//            Thread.Sleep(2000);
//            sut.Write("MV550");
//            Thread.Sleep(2000);
//            sut.Write("SI?");
//            Thread.Sleep(2000);
//            sut.Write("HD?");
//            sut.Write("HD?");
//            sut.Write("NSE");
//            sut.Write("NSE0Now Playing USB_????");
            Thread.Sleep(30000);
        }
    }
}
