using Akka.Actor;
using Akka.Configuration;
using Autofac;

namespace Obsession.Service.AutofacModules
{
    public class AkkaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var config = @"
            akka.remote.helios.tcp {
                transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
                transport-protocol = tcp
                port = 8080
                hostname = ""127.0.0.1""
            }";

            var system = ActorSystem.Create("Obsession", config);
            builder.RegisterInstance(system);
        }
    }

}
