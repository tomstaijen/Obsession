using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YahooWeather
{
    public interface IWeatherProvider
    {
        Forecast GetWeather();
    }

    public class WeatherConfiguration
    {
        public long WoeId { get; set; }
    }

    public class WeatherProvider : IWeatherProvider
    {
        private readonly YahooWeatherApi _api;
        private readonly WeatherConfiguration _configuration;

        public WeatherProvider(YahooWeatherApi api, WeatherConfiguration configuration)
        {
            _api = api;
            _configuration = configuration;
        }

        public Forecast GetWeather()
        {
            return _api.GetWeather(_configuration.WoeId);
        }
    }
}
