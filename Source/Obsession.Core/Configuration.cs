using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsession.Core
{
    /// <summary>
    /// Defines a module instance.
    /// A module is a piece of functionality that may be reused for different configurations.
    /// For example, you may use a weather module for different locations.
    /// The configuration would describe that you want to use the weather module, with specification configuration StateValues, and give it a unique name.
    /// </summary>
    public class Configuration
    {
        public Configuration()
        {
            Values = new Dictionary<string, object>();
        }

        /// <summary>
        /// This is the key to the state data.
        /// </summary>
        public string ObjectName { get; set; }

        public string ModuleName { get; set; }

        public IDictionary<string,object> Values { get; private set; }

        public bool Poll { get; set; }

        public Configuration WithValue(string key, object value)
        {
            Values.Add(key, value);
            return this;
        }
    }
}
