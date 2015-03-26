using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsession.Core
{
    public interface IServiceModule : IStateProvider
    {
        TimeSpan GetInterval();
        IDictionary<string, Delegate> GetActions();
    }
}
