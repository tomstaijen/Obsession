using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsession.Core
{
    public interface IServiceModule : IStateProvider
    {
        IModuleInstance GetInstance(Configuration configuration);
        TimeSpan GetInterval(Configuration configuration);
    }

    public interface IModuleInstance
    {
        StateValues GetState();
        IDictionary<string, Delegate> GetActions();
    }
}
