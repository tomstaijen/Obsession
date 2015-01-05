using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using YahooWeather;
using FluentAssertions;

namespace Test
{
    [TestFixture]
    class YahooWeatherTest
    {
        [Test]
        public void TestSearchPlace()
        {
            var sut = new YahooWeatherApi("nl-nl");
            var result = sut.SearchPlace("Elst");

            result.Should().NotBeEmpty();
        }

        [Test]
        public void TestGetWeather()
        {
            var sut = new YahooWeatherApi("nl-nl");
            var result = sut.GetWeather(729087);

            var temp = result.Item.Condition.TempCelcius;
            Console.WriteLine(temp);

            (result as object).Should().NotBeNull();
        }

    }
}
