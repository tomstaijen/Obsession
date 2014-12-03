using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Hosting;

namespace NancyOwinApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var url = "http://+:8080";

            using (WebApp.Start<StartUp>(url))
            {
                Console.WriteLine("Running on {0}", url);
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}
