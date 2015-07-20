using Autofac;
using InComfort;
using Obsession.Core;
using Obsession.Core.Extensions;

namespace Obsession.Service.AutofacModules
{
    class InComfortModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InComfortReaderService>();

            // transform the service into a func
            builder.RegisterType<InComfortReaderService>().Named<IServiceModule>("incomfort");

            builder.RegisterInstance(new Configuration("incomfort", "heating") { Poll = true, Persist = true }
                .WithValue("Hostname", "192.168.3.54"));
        }
    }
}
