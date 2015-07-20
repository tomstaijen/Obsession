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
        private static ILoggingAdapter _log = Logging.GetLogger(Context);

        private IServiceModule _serviceModule;
        private Configuration _configuration;
        private readonly IModuleFactory _moduleFactory;
        private Func<Owned<IPersister>> _persisterFunc;

        public PluginController(IModuleFactory moduleFactory, Func<Owned<IPersister>> persisterFunc)
        {
            if (persisterFunc == null) throw new ArgumentNullException("persisterFunc");
            _moduleFactory = moduleFactory;
            _persisterFunc = persisterFunc;
            Receive<PluginStart>(s => Start(s));
            Receive<PluginGetState>(s => GetState(s));
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(// or AllForOneStrategy
            maxNrOfRetries: 10,
            withinTimeRange: TimeSpan.FromSeconds(30),
            localOnlyDecider: x =>
            {
                // Maybe ArithmeticException is not application critical
                // so we just ignore the error and keep going.
                if (x is ArithmeticException) return Directive.Resume;

                // Error that we have no idea what to do with
                else if (x is InvalidProgramException) return Directive.Escalate;

                // Error that we can't recover from, stop the failing child
                else if (x is NotSupportedException) return Directive.Stop;

                // otherwise restart the failing child
                else return Directive.Restart;
            });
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
