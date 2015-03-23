using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using Core;
using InComfort;
using Microsoft.Owin.Hosting;
using Obsession.Core;
using Obsession.Core.Effectors;
using P1Reader;

namespace Obsession.Service
{
    public class AppStartUp
    {
        public static IDisposable TheWebApp { get; set; }

        public void Start()
        {
            TheWebApp = WebApp.Start<WebAppStartUp>("http://+:5533");

            var container = Bootstrapper.GetContainer();
            var system = container.Resolve<ActorSystem>();

            IDependencyResolver propsResolver = new AutoFacDependencyResolver(container, system);

            var eventActor = system.ActorOf(propsResolver.Create<EventProcessor>());
            system.EventStream.Subscribe(eventActor, typeof (StateChanged));

            var actor = system.ActorOf(propsResolver.Create<PluginManager>());
            actor.Tell(PluginManager.Start);

        }

        public void Stop()
        {
            TheWebApp.Dispose();
            var container = Bootstrapper.GetContainer();
            container.Resolve<ActorSystem>().Shutdown();
        }

    }
}
