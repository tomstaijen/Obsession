using System;
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
        private readonly IIndex<string, IServiceModule> _modules;

        private IDictionary<string, ActorRef> _pluginActors = new Dictionary<string, ActorRef>(); 

        public PluginManager(Func<Owned<IStore<Configuration>>> configStoreFactory, IIndex<string, IServiceModule> modules)
        {
            _configStoreFactory = configStoreFactory;
            _modules = modules;

            Receive<PluginManagerStart>(Handle);
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

                    var timestamp = _modules[config.ModuleName].GetInterval(config);

                }
            }
            return true;
        }
    }

    public class PluginManagerStart { }
}
