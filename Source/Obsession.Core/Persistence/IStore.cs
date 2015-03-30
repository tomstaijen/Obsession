using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Obsession.Core.Rules;

namespace Obsession.Core.Persistence
{
    public interface IStore<out T> where T : class
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

    public class StaticRuleStore : IStore<Rule>
    {
        public IEnumerable<Rule> GetThem()
        {
            return new[]
                {
                    new Rule()
                        {
                            Name = "Hello",
                            Script =
@"
if 
    heating.RoomTemp1 < 20
then 
    notify(heating.RoomTemp1)
    heating.setTemp(20.5)
    notify('set temp to 20.5')
end
"
                        }
                };
        }
    }
}
