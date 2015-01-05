using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YahooWeather
{
    public class YahooWeatherApi
    {
        private string _language = "en-US";
        public YahooWeatherApi(string language)
        {
            _language = language;
        }

        public IEnumerable<Place> SearchPlace(string name)
        {
            var result = DoIt(string.Format("select * from geo.places where text = \"{0}\" and lang = \"{1}\"", name, _language));
            var array =  (result.query.results.place as JArray);
            return array.Select(o => (o as JObject).ToObject<Place>());
        }

        public Forecast GetWeather(long woeid)
        {
            var result = DoIt(string.Format("select * from weather.forecast where woeid={0}", woeid));

            return (result.query.results.channel as JObject).ToObject<Forecast>();
        }

        public dynamic DoIt(string query)
        {
            var client = new HttpClient();
            // auth
            // content-type
            // do it!

            var builder = new UriBuilder("https://query.yahooapis.com/v1/public/yql");
            var q = HttpUtility.ParseQueryString(builder.Query);
            q["q"] = query;
            q["format"] = "json";
            q["diagnostics"] = "true";
            builder.Query = q.ToString();

            var result = client.GetAsync(builder.ToString()).Result;

            if (result.StatusCode != HttpStatusCode.OK)
                throw new Exception(result.StatusCode.ToString());

            var resultValue = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result) as dynamic;

            return resultValue;
        }
    }
}
