using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Configuration
    {
        public string Name { get; set; }
        public ICollection<ConfigurationValue> Values { get; set; }
    }

    public class ConfigurationValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

}
