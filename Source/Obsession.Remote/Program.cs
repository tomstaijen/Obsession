using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Topshelf;

namespace Obsession.Remote
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(h =>
            {
                h.Service<AppStartUp>(c =>
                {
                    c.ConstructUsing(sf => new AppStartUp());
                    c.WhenStarted(s => s.Start());
                    c.WhenStopped(s => s.Stop());
                });
                h.SetServiceName("ObsessionRemote");
                h.SetDescription("ObsessionRemote");
                h.SetDisplayName("ObsessionRemote");
            });

            Console.ReadLine();
        }
    }

    public class AppStartUp
    {
        public class MyActor : ReceiveActor
        {
            public MyActor()
            {
                Receive<string>(s =>
                    {
                        Console.WriteLine(s);
                    });
            }
        }


        public void Start()
        {
            var config = @"
log-config-on-start = on
stdout-loglevel = DEBUG
loglevel = DEBUG
actor {
    provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
}
remote {
        helios.tcp {
		    port = 8090
		    hostname = localhost
        }
}
";
            var system = ActorSystem.Create("A", config);
            system.Log.Info("Hello");

            var scope = new RemoteScope(new Address("akka.tcp", "Obsession", "localhost", 8080));
            var deploy = new Deploy(scope);

            var actor = system.ActorOf(Props.Create<MyActor>().WithDeploy(deploy));
            actor.Tell("Hello");

        }

        public void Stop()
        {
            
        }
    }
}
