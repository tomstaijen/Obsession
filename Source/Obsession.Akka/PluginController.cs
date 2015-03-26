using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using Autofac.Features.Indexed;
using Autofac.Features.OwnedInstances;
using Obsession.Core.Persistence;

namespace Obsession.Core.Effectors
{
    public class PluginController : ReceiveActor
    {
        private static LoggingAdapter _log = Logging.GetLogger(Context);

        private IServiceModule _serviceModule;
        private Configuration _configuration;
        private readonly IModuleFactory _moduleFactory;
        private Func<Owned<IPersister>> _persisterFunc;

        public PluginController(IModuleFactory moduleFactory, Func<Owned<IPersister>> persisterFunc)
        {
            if (persisterFunc == null) throw new ArgumentNullException("persisterFunc");
            _moduleFactory = moduleFactory;
            _persisterFunc = persisterFunc;
            Receive<PluginStart>(Start);
            Receive<PluginGetState>(GetState);
        }

        public bool Start(PluginStart message)
        {
            _serviceModule = _moduleFactory.Create(message.Configuration);
            _configuration = message.Configuration;

            Self.Tell(new PluginGetState());
            
            return true;
        }

        public bool GetState(PluginGetState message)
        {
            try
            {
                var state = _serviceModule.GetState();
                if (state != null)
                {
                    if (_configuration.Persist)
                    {
                        using (var p = _persisterFunc())
                        {
                            p.Value.Put(state);
                        }
                    }

                    Context.System.EventStream.Publish(new StateChanged
                        {
                            Configuration = _configuration,
                            State = state
                        });
                }
            }
            catch (Exception e)
            {
                _log.Error(e, "Error getting state from {0}@{1}", _configuration.ObjectName, _configuration.ModuleName);
            }

            Thread.Sleep(_serviceModule.GetInterval());

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
