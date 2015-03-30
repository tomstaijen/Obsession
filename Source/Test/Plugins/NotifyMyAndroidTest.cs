using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NotifyMyAndroid;

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
            Assert.True(nma.Notify(apikey, "Test", "Hello", "This is a test message."));
        }
    }
}
