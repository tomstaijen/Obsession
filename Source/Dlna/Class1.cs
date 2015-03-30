using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedUPnP;

namespace Dlna
{
    public class UpnpLister
    {
        public string[] Test()
        {
            bool completed;
            var disc = Discovery.FindDevices(null, 10000, 10000, out completed).ToList();
            
            foreach (var d in disc)
            {
                Console.WriteLine(d.Description);
                Console.WriteLine(d.DocumentURL);
                Console.WriteLine(d.UniqueDeviceName);
                Console.WriteLine(d.SerialNumber);
                Console.WriteLine(d.Type);
                Console.WriteLine("--------");
            }
            return disc.Select(d => d.Description).ToArray();
        }

    }
}
