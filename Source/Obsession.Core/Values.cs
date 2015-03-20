using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsession.Core
{
    public class Values : IValues
    {
        private IDictionary<string,object> _dictionary = new Dictionary<string, object>();

        public void AddValue(string key, object value)
        {
            _dictionary[key] = value;
        }

        public IDictionary<string, object> GetValues()
        {
            return _dictionary;
        }
    }
}
