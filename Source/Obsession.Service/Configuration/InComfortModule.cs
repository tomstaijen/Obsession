using System;
using Autofac;
using InComfort;
using Obsession.Core;

namespace Obsession.Service.Configuration
{
    class InComfortModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // configuration
            builder.RegisterInstance(new InComfortConfiguration {Host = "192.168.3.55"});
            
            // api
            builder.RegisterType<InComfortReaderService>();

            // transform the service into a func
            builder.Register(c => c.Resolve<InComfortReaderService>().Read()).As<ReadableInComfortData>();

            // this is a singleton, we don't want to loose it's current state
//            builder.RegisterType<ExpirationUpdater<ReadableInComfortData>>().As<IStateProvider<ReadableInComfortData>>().SingleInstance();
//
//            builder.RegisterInstance(new Expiration<ReadableInComfortData>()
//                {
//                    Value = new TimeSpan(0, 0, 0, 30)
//                });
//
//            builder.RegisterType<InComfortParamValueProvider>().As<IParamValueProvider>();
        }
    }
}
