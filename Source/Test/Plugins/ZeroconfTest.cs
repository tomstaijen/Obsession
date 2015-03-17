using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    class ZeroconfTest
    {
        [Test]
        public void Test()
        {
            var xss = new Zeroconf.Resolver().Resolve();
            foreach (var xs in xss)
            {
                foreach (var x in xs)
                {
                    Console.WriteLine("{0} - {1}", xs.Key, x);
                }
            }
            xss.Should().NotBeEmpty();
        }
    }
}
