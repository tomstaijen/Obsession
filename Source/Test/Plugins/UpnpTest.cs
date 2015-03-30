using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dlna;
using NUnit.Framework;

namespace Test.Plugins
{
    [TestFixture]
    public class UpnpTest
    {
        [Test]
        public void Test()
        {
            var x = new UpnpLister();
            var devices = x.Test();
            foreach (var device in devices)
            {
                Console.WriteLine(device);
            }
            Assert.That(devices.Any());

        }
    }
}
