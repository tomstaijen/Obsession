using Akka.Actor;
using Autofac;

namespace Obsession.Service.AutofacModules
{
    public class AkkaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var system = ActorSystem.Create("Obsession");
            builder.RegisterInstance(system);
        }
    }

}
