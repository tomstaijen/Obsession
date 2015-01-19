using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsession.Core
{
    public interface IStateProvider<T>
    {
        T GetCurrent();
    }
}
