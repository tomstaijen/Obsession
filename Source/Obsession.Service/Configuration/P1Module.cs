using Akka.Actor;
using Autofac;
using P1Reader;

namespace Obsession.Service.Configuration
{
    class P1Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

        }

        public void Register(ActorSystem system)
        {
            var props = Props.Create<P1Actor>();
            var p1Ref = system.ActorOf(props);
        }
    }

    
}
