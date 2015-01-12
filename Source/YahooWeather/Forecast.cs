using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YahooWeather
{
    public class Forecast
    {
        public LocationType Location { get; set; }
        public ItemType Item { get; set; }

        public class ItemType
        {
            public string Description { get; set; }
            public ConditionType Condition { get; set; }
            public ForecastType[] Forecast { get; set; }    
        }

        [DebuggerDisplay("{City} {Region} {Country}")]
        public class LocationType
        {
            public string City { get; set; }
            public string Country { get; set; }
            public string Region { get; set; }

            public override string ToString()
            {
                return string.Format("{0} ({1}) - {2}", City, Region, Country);
            }
        }

        public class ConditionType
        {
            public short Code { get; set; }
            public string Date { get; set; }
            public short Temp { get; set; }
            public string Text { get; set; }

            public double TempCelcius
            {
                get { 
                    var temp = (Convert.ToDouble(Temp) - 32)/1.8;

                    temp = temp*2;
                    temp = Math.Round(temp, 0);
                    temp = temp/2;
                    return temp;
                }
            }

        }

        public class ForecastType
        {
            public string Code { get; set; }
            public string Date { get; set; }
            public string Day { get; set; }
            public string high { get; set; }
            public string Low { get; set; }
            public string Text { get; set; }
        }
    }
}
