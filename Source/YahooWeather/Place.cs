using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YahooWeather
{
    [DebuggerDisplay("{WoeId} - {AsString}")]
    public class Place
    {
        public string Name { get; set; }
        public long WoeId { get; set; }
        
        public PlaceSpec Locality1 { get; set; }
        public PlaceSpec Locality2 { get; set; }
        public PlaceSpec Postal { get; set; }

        public PlaceSpec Country { get; set; }
        public PlaceSpec Admin1 { get; set; }
        public PlaceSpec Admin2 { get; set; }
        public PlaceSpec Admin3 { get; set; }

        public class PlaceSpec
        {
            public string Code { get; set; }
            public string Type { get; set; }
            public long WoeId { get; set; }
            public string Content { get; set; }
        }

        public string AsString
        {
            get { return ToString(); }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1} ({2})", Locality1.Content, Admin1.Content, Country.Code);
        }
    }
}
