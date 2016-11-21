using Akka.Actor;
using Autofac;
using Obsession.Core;
using Obsession.Core.Extensions;
using P1Reader;

namespace Obsession.Service.AutofacModules
{
    class P1Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<P1ServiceServiceModule>().Named<IServiceModule>("p1");

            builder.RegisterInstance(new Configuration("p1", "p1"){ Poll = true }.WithValue("Hostname", "192.168.3.28"));

            builder.RegisterType<P1ReaderService>().InstancePerLifetimeScope();
        }
    }

    
}
