using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsession.Core.Persistence
{
    public interface IStatePersister
    {
        // persist a state for some configuration
        void Persist<T>(Configuration configuration, T state);
    }
}
