using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsession.Core.Params
{
    public class NumericParam : Param
    {
        public double? Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
