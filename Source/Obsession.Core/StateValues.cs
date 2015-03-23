using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Obsession.Core.Helpers;

namespace Obsession.Core
{
    public class StateValues : IValues
    {
        // auto convert object's properties to dictionary values
        public StateValues(string module, string instance, object o) : this(module, instance)
        {
            _dictionary = o != null ? o.ToDictionary() : new Dictionary<string, object>();
        }

        public StateValues(string module, string instance)
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.Now;
            Module = module;
            Instance = instance;
        }

        public Guid Id { get; set; }
        public string Module { get; private set; }
        public string Instance { get; private set; }
        private IDictionary<string, object> _dictionary = new Dictionary<string, object>();


        [JsonProperty("@timestamp")]
        public string TimestampISO
        {
            get
            {
                return Timestamp.ToUniversalTime().ToString("o");
            }
        }
        [JsonIgnore]
        public DateTime Timestamp { get; set; }



        public void AddValue(string key, object value)
        {
            _dictionary[key] = value;
        }


        public IDictionary<string, object> Values
        {
            get
            {
                return _dictionary;
            }
        }
    }
}
