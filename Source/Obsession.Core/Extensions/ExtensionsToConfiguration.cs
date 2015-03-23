using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsession.Core.Extensions
{
    public static class ExtensionsToConfiguration
    {
        public static string PersistStateKey = "PersistState";

        public static bool PersistState(this Configuration config)
        {
            return true;
        }
    }
}
