using System.Linq;
using Bluetooth;
using NUnit.Framework;
using Obsession.Core;

namespace Tests
{
    public class BtTests
    {
        [Test]
        public void BluetoothTest()
        {
            var result = new BluetoothService(new Configuration("hi", "tom")).GetState();
            Assert.That(result.Values.Any());
        }
    }
}