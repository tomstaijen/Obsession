using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Topshelf;

namespace Obsession.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(h =>
                {
                    h.Service<AppStartUp>(c =>
                        {
                            c.ConstructUsing(sf => new AppStartUp());
                            c.WhenStarted(s => s.Start());
                            c.WhenStopped(s => s.Stop());
                        });
                    h.SetServiceName("Obsession");
                    h.SetDescription("Obsession");
                    h.SetDisplayName("Obsession");
                });

            Console.ReadLine();
        }
    }
}
