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
//            var result = sut.GetActivePlayers();

            Console.WriteLine(sut.GetInfo());
            //sut.Navigate().Left();

//            var result = sut.PlayUrl("plugin://plugin.video.youtube/play/?video_id=logaMSPVV-E");
//            sut.PlayPause(1);
            var movieInfo = sut.GetMovies();
            foreach (var m in movieInfo)
            {
                Console.WriteLine(m);
            }

            var id = (long) movieInfo.movies[0].movieid;

            var movie = sut.GetMovieDetails(id);

            Console.WriteLine(movie);
//            Console.WriteLine(result);
//
//            foreach (dynamic player in result)
//            {
//                var items = sut.GetItemVideo(player.playerid.Value);
//
//                foreach (dynamic item in items)
//                {
//                    Console.WriteLine(item);    
//                }
//
//                var properties = sut.GetPlayerProperties(player.playerid.Value);
//                Console.WriteLine(properties);
//            }
        }
    }
}
