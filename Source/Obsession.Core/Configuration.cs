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
    /// The configuration would describe that you want to use the weather module, with specification configuration values, and give it a unique name.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// This is the key to the state data.
        /// </summary>
        public string ObjectName { get; set; }

        public string ModuleName { get; set; }

        public IValues Values { get; set; }
    }
}
