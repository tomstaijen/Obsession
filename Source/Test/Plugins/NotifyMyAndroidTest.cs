using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bluetooth;
using FluentAssertions;
using InComfort;
using NUnit.Framework;
using NotifyMyAndroid;
using Obsession.Core;

namespace Tests
{
    public class NotifyMyAndroidTest
    {
        [Test]
        public void CanNotifyMyAndroid()
        {
            var nma = new NotifyMyAndroidService();
            string apikey = "9bdad31063771a8b755088ed25ac4aa00034aad44442d0a3";
            Assert.True(nma.Verify(apikey));
            Assert.True(nma.Notify(apikey, "Test","Hello", "This is a test message."));
        }

        [Test]
        public void CanReadInComfort()
        {
            var nma = new InComfortService();
            var result = nma.Read("192.168.3.55");

            result.Should().NotBeNull();
        }

        [Test]
        public void BluetoothTest()
        {
            new BluetoothService().Test();
        }
    }
}
