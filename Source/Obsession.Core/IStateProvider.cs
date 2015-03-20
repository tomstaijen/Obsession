using System.Collections.Generic;
using Obsession.Core.Persistence;

namespace Obsession.Core
{
    public interface IStateProvider
    {
        IValues GetState(Configuration configuration);
    }

    public interface IStateManager
    {
        IValues GetActualState();
    }

    /// <summary>
    /// Should this be an actor? It's a lot of coordination
    /// </summary>
    public class StateManager : IStateManager
    {
        private readonly IStore<Configuration> _configStore;
        private readonly IDictionary<string, IStateProvider> _retrievers;

        public StateManager(IStore<Configuration> configStore, IDictionary<string, IStateProvider> retrievers)
        {
            _configStore = configStore;
            _retrievers = retrievers;
            _retrievers = retrievers;
        }

        public IValues GetActualState()
        {
            var result = new Values();

            var configs = _configStore.GetThem();
            foreach (var config in configs)
            {
                result.AddValue(config.ObjectName, _retrievers[config.ModuleName].GetState(config));
            }
            
            return result;
        }
    }
}