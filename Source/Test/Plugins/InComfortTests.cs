using FluentAssertions;
using InComfort;
using NUnit.Framework;

namespace Tests
{
    public class InComfortTests
    {
        [Test]
        public void CanReadInComfort()
        {
            var nma = new InComfortService();
            var result = nma.Read("192.168.3.55");

            result.Should().NotBeNull();
        }
    }
}