using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsession.Core
{
    public class ScriptException : Exception
    {
        public int? Line { get; set; }

        public ScriptException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}
