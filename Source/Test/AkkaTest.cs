using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.AutoFac;
using Autofac;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class AkkaTest
    {
        [Test]
        public void Test()
        {
            // setup
            var system = ActorSystem.Create("TestSystem");
            var builder = new ContainerBuilder();
            builder.RegisterType<MyActor>();
            builder.RegisterType<MyService>().As<IService>();

            builder.RegisterType<Func<Props, IActorRef>>();

            var container = builder.Build();

            var resolver = new AutoFacDependencyResolver(container, system);

            var actorRef = system.ActorOf(resolver.Create<MyActor>(),"pietje");
            actorRef.Tell(new MyActorStart
                {
                    Sender = "Tom"
                });

            system.AwaitTermination();
        }
    }

    public class MyActorStart
    {
        public string Sender { get; set; }
    }

    public class MyActor : ReceiveActor
    {
        private readonly IService _service;

        public MyActor(IService service)
        {
            _service = service;

            Receive<MyActorStart>(s => Run(s.Sender));
        }

        public void Run(string name)
        {
            _service.DoService(name);
        }
    }

    public interface IActorFactory<T> where T : ActorBase
    {
        IActorRef Create();
    }

    public interface IService
    {
        void DoService(string someone);
    }

    public class MyService : IService
    {
        public void DoService(string someone)
        {
            Console.WriteLine("Hello {0}, this is MyService. How are you?", someone);
        }
    }
}
