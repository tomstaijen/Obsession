using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeroconf
{
    public class Resolver
    {
        public ILookup<string,string> Resolve()
        {
            return Zeroconf.ZeroconfResolver.BrowseDomainsAsync().Result;
        }
    }
}
