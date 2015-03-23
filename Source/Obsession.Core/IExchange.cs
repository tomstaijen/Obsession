using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsession.Core
{
    interface IExchange
    {
        void Publish<T>(T message) where T : class;
    }
}
