using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using InComfort;
using NUnit.Framework;
using NotifyMyAndroid;

namespace Tests
{
    public class Class1
    {
        [Test]
        public void CanNotifyMyAndroid()
        {
            var nma = new NotifyMyAndroidPlugin();

            Assert.True(nma.Verify("9bdad31063771a8b755088ed25ac4aa00034aad44442d0a3"));
            Assert.True(nma.Notify("This is a test message."));
        }

        [Test]
        public void CanReadInComfort()
        {
            var nma = new InComfortReader();
            var result = nma.Read();

            result.Should().NotBeNull();
        }
    }
}
