using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Obsession.Core;
using YahooWeather;

namespace Obsession.Service.Configuration
{
    public class YahooWeatherModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // providers the api to yahoo
            builder.RegisterType<YahooWeatherApi>().SingleInstance();
            
            // provides the configuration
            builder.RegisterType<WeatherProvider>().As<IWeatherProvider>();
            
            // transform the provider into a func
            builder.Register(c => c.Resolve<IWeatherProvider>().GetWeather()).As<Forecast>();

            // store and update the current version
            builder.RegisterType<AutoUpdater<Forecast>>().As<IStateProvider<Forecast>>();
            // configure the expiration
            builder.RegisterInstance(new Expiration<Forecast> {Value = new TimeSpan(0, 0, 15, 0)});

            // map the forecast to a bunch of rule parameters
            builder.RegisterType<WeatherParamValueProvider>().As<IParamValueProvider>();
        }
    }
}
