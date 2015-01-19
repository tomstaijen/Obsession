using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Obsession.Core.Params;

namespace Obsession.Core
{
    public interface IParamValueProvider
    {
        IEnumerable<Param> GetParams();
    }
}
