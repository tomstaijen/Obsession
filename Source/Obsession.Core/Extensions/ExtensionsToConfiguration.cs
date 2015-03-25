using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsession.Core.Extensions
{
    public static class ExtensionsToConfiguration
    {
        public static Configuration WithValue(this Configuration config, string key, object value)
        {
            config.Values.Add(key, value);
            return config;
        }

        public static T GetValue<T>(this Configuration config, string key, T def = default(T))
        {
            if (!config.Values.ContainsKey(key))
                return def;
            var value = config.Values[key];

            if (value is T)
                return (T) value;
            return def;
        }
        
    }
}
