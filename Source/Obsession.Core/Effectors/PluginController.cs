using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Autofac.Features.Indexed;
using Autofac.Features.OwnedInstances;
using Obsession.Core.Persistence;

namespace Obsession.Core.Effectors
{
    public class PluginController : ReceiveActor
    {
        private readonly IIndex<string, IServiceModule> _modules;
        

        private IServiceModule _serviceModule;
        private Configuration _configuration;
        private Func<Owned<IPersister>> _persisterFunc;

        public PluginController(IIndex<string,IServiceModule> modules, Func<Owned<IPersister>> persisterFunc)
        {
            if (persisterFunc == null) throw new ArgumentNullException("persisterFunc");
            _modules = modules;
            _persisterFunc = persisterFunc;
            _modules = modules;
            Receive<PluginStart>(Start);
            Receive<PluginGetState>(GetState);
        }

        public bool Start(PluginStart message)
        {
            _serviceModule = _modules[message.Configuration.ModuleName];
            _configuration = message.Configuration;


            Self.Tell(new PluginGetState());
            
            return true;
        }

        public bool GetState(PluginGetState message)
        {
            var instance = _serviceModule.GetInstance(_configuration);
            var state = instance.GetState();
            if (state != null)
            {
                using (var p = _persisterFunc())
                {
                    p.Value.Put(state);
                    Context.System.EventStream.Publish(new StateChanged
                        {
                            Configuration = _configuration,
                            State = state
                        });
                }
            }

            Self.Tell(new PluginGetState());

            return true;
        }

    }

    public class PluginGetState { }

    public class PluginStart
    {
        public Configuration Configuration { get; set; }
    }
}
