using Akka.Actor;
using Autofac;
using Obsession.Core;
using P1Reader;

namespace Obsession.Service.AutofacModules
{
    class P1Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<P1ServiceServiceModule>().Named<IServiceModule>("p1");

            builder.RegisterInstance(new Configuration(){ModuleName = "p1", ObjectName = "p1", Poll = true }.WithValue("Hostname", "192.168.3.29"));

            builder.RegisterType<P1ReaderService>().InstancePerLifetimeScope();
        }
    }

    
}
