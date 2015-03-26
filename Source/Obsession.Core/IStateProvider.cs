using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using Obsession.Core.Extensions;
using Obsession.Core.Persistence;

namespace Obsession.Core
{
    public interface IStateProvider
    {
        StateValues GetState();
        bool IsActual(StateValues current);
    }

    public interface IStateManager
    {
        IDictionary<string,object> GetActualState();
        StateValues GetActualState(Configuration config);
        void SetState(Configuration configuration, StateValues values);
    }

    /// <summary>
    /// Should this be an actor? It's a lot of coordination
    /// </summary>
    public class StateManager : IStateManager
    {
        private readonly IStore<Configuration> _configStore;
        private readonly IModuleFactory _moduleFactory;
        private readonly IDictionary<string, StateValues> _currentValues = new Dictionary<string, StateValues>();

        public StateManager(IStore<Configuration> configStore, IModuleFactory moduleFactory)
        {
            _configStore = configStore;
            _moduleFactory = moduleFactory;
        }

        public void SetState(Configuration configuration, StateValues values)
        {
            _currentValues.SetOrAdd(configuration.ObjectName, values);
        }

        public StateValues GetActualState(Configuration config)
        {
            var current = _currentValues.GetValueOrDefault(config.ObjectName);
            var module = _moduleFactory.Create(config);
            if ((current == null || !module.IsActual(current)) && config.Poll)
            {
                current = module.GetState();
                if (current != null)
                    SetState(config, current);
            }
            return current;
        }

        public IDictionary<string,object> GetActualState()
        {
            var configs = _configStore.GetThem();

            var result = new Dictionary<string, object>();
            foreach (var config in configs)
            {
                var current = GetActualState(config);
                if (current != null)
                    result.Add(config.ObjectName, current.Values);

            }
            return result;
        }
    }
}