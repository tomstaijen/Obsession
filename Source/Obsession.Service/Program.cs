using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace Obsession.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://+:5533";

            using (WebApp.Start<WebAppStartUp>(url))
            {
                Console.WriteLine("Running on {0}", url);
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}
