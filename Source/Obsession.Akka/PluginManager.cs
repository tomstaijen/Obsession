﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Autofac.Features.Indexed;
using Autofac.Features.OwnedInstances;
using Obsession.Core.Extensions;
using Obsession.Core.Persistence;

namespace Obsession.Core.Effectors
{
    public class PluginManager : ReceiveActor
    {
        public static PluginManagerStart Start = new PluginManagerStart();

        private readonly Func<Owned<IStore<Configuration>>> _configStoreFactory;
        private IDictionary<string, IActorRef> _pluginActors = new Dictionary<string, IActorRef>(); 

        public PluginManager(Func<Owned<IStore<Configuration>>> configStoreFactory)
        {
            _configStoreFactory = configStoreFactory;

            Receive<PluginManagerStart>(m => Handle(m));
        }

        public bool Handle(PluginManagerStart start)
        {
            using (var store = _configStoreFactory())
            {
                var configs = store.Value.GetThem();
                foreach (var config in configs.Where(c => c.Poll))
                {
                    var actor = Context.Resolve<PluginController>();
                    actor.Tell(new PluginStart { Configuration = config });
                    _pluginActors.Add(config.ObjectName, actor);

                    //var timestamp = _modules[config.ModuleName].GetInterval(config);

                }
            }
            return true;
        }
    }

    public class PluginManagerStart { }
}
