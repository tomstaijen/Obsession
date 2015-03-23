using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsession.Core
{
    public interface IEngine
    {
        void RegisterContext(string key, object value);
        void RegisterContext(IDictionary<string, object> context);
        object Run(string script);
    }
}
