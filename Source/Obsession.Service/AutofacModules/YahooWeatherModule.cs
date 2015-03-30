using Autofac;
using Obsession.Core;
using Obsession.Core.Extensions;
using YahooWeather;

namespace Obsession.Service.AutofacModules
{
    public class YahooWeatherModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // register the configuration
            builder.RegisterInstance(new Configuration("weather", "weather")
                { Poll = true, Persist = true }
                .WithValue(WeatherProvider.Language, "nl-nl")
                .WithValue(WeatherProvider.LocationId, (long?) 729087));

            // provides the configuration to the api
            builder.RegisterType<WeatherProvider>().Named<IServiceModule>("weather");
        }
    }
}
