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
        public Configuration(string moduleName, string objectName)
        {
            Values = new Dictionary<string, object>();
            ModuleName = moduleName;
            ObjectName = objectName;
            Persist = true;
        }

        /// <summary>
        /// This is the key to the state data.
        /// </summary>
        public string ObjectName { get; private set; }

        public string ModuleName { get; private set; }

        public IDictionary<string,object> Values { get; private set; }

        /// <summary>
        /// Should the state be polled? Default = false
        /// </summary>
        public bool Poll { get; set; }

        /// <summary>
        /// Should the state be persisted? Default = true
        /// </summary>
        public bool Persist { get; set; }
    }
}
