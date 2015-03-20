using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsession.Core.Persistence
{
    public interface IStore<out T>
    {
        IEnumerable<T> GetThem();
    }
    
    /// <summary>
    /// For debugging purposed, to hardcode some configuration, use this as configstore.
    /// </summary>
    public class InjectableConfigStore : IStore<Configuration>
    {
        private readonly IEnumerable<Configuration> _configurations;

        public InjectableConfigStore(IEnumerable<Configuration> configurations)
        {
            _configurations = configurations;
        }

        public IEnumerable<Configuration> GetThem()
        {
            return _configurations;
        }
    }
}
