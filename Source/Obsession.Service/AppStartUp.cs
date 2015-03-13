using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using InComfort;
using Microsoft.Owin.Hosting;
using P1Reader;

namespace Obsession.Service
{
    public class AppStartUp
    {
        public static IDisposable TheWebApp { get; set; }

        public static List<IService> _services = new List<IService>();

        public void Start()
        {
            TheWebApp = WebApp.Start<WebAppStartUp>("http://+:5534");

            var s1 = new P1ReaderService()
                {
                    Host = "192.168.3.29"
                };
            new Task(s1.Start).Start();

            _services.Add(s1);

            var s2 = new InComfortReaderService(new InComfortConfiguration()
                {
                    Host = "192.168.3.55"
                });

            new Task(s2.Start).Start();

            _services.Add(s2);
        }

        public void Stop()
        {
            TheWebApp.Dispose();
            _services.ForEach(s => s.Stop());
        }

    }
}
