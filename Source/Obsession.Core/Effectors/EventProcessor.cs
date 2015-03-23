using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Autofac.Features.Indexed;
using Autofac.Features.OwnedInstances;
using Obsession.Core.Persistence;
using Obsession.Core.Rules;

namespace Obsession.Core.Effectors
{
    public class EventProcessor : ReceiveActor
    {
        private readonly IStateManager _stateManager;
        private readonly Func<Owned<IEngine>> _engineFactory;
        private readonly IStore<Rule> _ruleStore;
        private readonly IEngineContextProvider _engineContextProvider;

        public EventProcessor(Func<Owned<IEngine>> engineFactory, IStore<Rule> ruleStore, IEngineContextProvider engineContextProvider, IStateManager stateManager)
        {
            _engineFactory = engineFactory;
            _ruleStore = ruleStore;
            _engineContextProvider = engineContextProvider;
            _stateManager = stateManager;

            Receive<StateChanged>(HandleStateChange);
        }

        /// <summary>
        /// TODO: move to per rule evaluation once the state has been settled
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool HandleStateChange(StateChanged message)
        {
            _stateManager.SetState(message.Configuration, message.State);
            
            using (var engineHolder = _engineFactory())
            {
                var engine = engineHolder.Value;
                var context = _engineContextProvider.GetContext();
                context.Add("notify", (Action<string>) Notify);
                engine.RegisterContext(context);
                foreach (var rule in _ruleStore.GetThem())
                {
                    engine.Run(rule.Script);
                }
            }
            return true;
        }

        public void Notify(string message)
        {
            Console.WriteLine("Script notification: " + message);
        }

    }

    public interface IEngineContextProvider
    {
        IDictionary<string, object> GetContext();
    }

    public class EngineContextProvider : IEngineContextProvider
    {
        private readonly IStateManager _manager;
        private readonly IIndex<string, IServiceModule> _modules;
        private readonly IStore<Configuration> _configurations;

        public EngineContextProvider(IStateManager manager, IIndex<string, IServiceModule> modules, IStore<Configuration> configurations)
        {
            _manager = manager;
            _modules = modules;
            _configurations = configurations;
        }

        public IDictionary<string, object> GetContext()
        {
            var result = new Dictionary<string, object>();
            foreach (var config in _configurations.GetThem())
            {
                var moduleResult = new Dictionary<string, object>();
                var module = _modules[config.ModuleName].GetInstance(config);
                var state = _manager.GetActualState(config);
                if (state != null)
                {
                    foreach (var a in state.Values)
                    {
                        moduleResult.Add(a.Key, a.Value);
                    }

                }
                var actions = module.GetActions();
                if (actions != null)
                {
                    foreach (var a in actions)
                    {
                        moduleResult.Add(a.Key, a.Value);
                    }
                }
                result.Add(config.ObjectName, moduleResult);
            }
            return result;
        }
    }

    public class StateChanged
    {
        public Configuration Configuration { get; set; }
        public StateValues State { get; set; }
    }
}
