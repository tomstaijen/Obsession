using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Xbmc;

namespace Test
{
    [TestFixture]
    public class XbmcTest
    {
        [Test]
        public void Test()
        {
            var sut = new Communicator("192.168.3.107", "kodi", "kodi");
            var result = sut.GetActivePlayers();

            sut.Navigate().Left();

            foreach (dynamic player in result)
            {
                var items = sut.GetItemVideo(player.playerid.Value);

                foreach (dynamic item in items)
                {
                    Console.WriteLine(item);    
                }

                var properties = sut.GetPlayerProperties(player.playerid.Value);
                Console.WriteLine(properties);
            }
        }
    }
}
