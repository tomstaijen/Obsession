using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Obsession.Core;

namespace YahooWeather
{

    public class WeatherProvider : IServiceModule
    {
        public static string Language = "Language";
        public static string LocationId = "LocationId";

        private readonly Configuration _configuration;
        private readonly YahooWeatherApi _api;

        public WeatherProvider(Configuration configuration)
        {
            _configuration = configuration;
            _api = new YahooWeatherApi(_configuration.Values[Language] as string);
        }

        public Forecast GetWeather()
        {
            var location = _configuration.Values[LocationId] as long?;
            if (location.HasValue)
            {
                return _api.GetWeather(location.Value);
            }
            return null;
        }

        public StateValues GetState()
        {
            var forecast = GetWeather();
            if (forecast != null)
            {
                return new StateValues(_configuration.ModuleName, _configuration.ObjectName)
                    .AddValue("Location", forecast.Location.City)
                    .AddValue("Temperature", forecast.Item.Condition.TempCelcius)
                    .AddValue("Description", forecast.Item.Condition.Text);
            }
            return null;
        }

        /// <summary>
        /// Valid for 30 minutes
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public bool IsActual(StateValues current)
        {
            return current.Timestamp > DateTime.Now.AddMinutes(-30);
        }

        public TimeSpan GetInterval()
        {
            return TimeSpan.FromHours(1);
        }

        public IDictionary<string, Delegate> GetActions()
        {
            return null;
        }
    }
}
