using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsession.Core
{
    public class RuleActionAttribute : Attribute
    {
        private readonly string _actionName;

        public RuleActionAttribute(string actionName)
        {
            _actionName = actionName;
        }

        public string ActionName { get { return _actionName; } }
    }
}
