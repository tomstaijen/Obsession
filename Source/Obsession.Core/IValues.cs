using System.Collections.Generic;

namespace Obsession.Core
{
    public interface IValues
    {
        IDictionary<string, object> GetValues();
    }
}