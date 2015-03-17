using System.Collections.Generic;

namespace Obsession.Core.Rules
{
    public interface IRuleStore
    {
        IEnumerable<Rule> GetRules();
    }
}
