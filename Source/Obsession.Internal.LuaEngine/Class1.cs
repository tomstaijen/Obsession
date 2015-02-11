using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo.IronLua;

namespace Obsession.Internal.LuaEngine
{
    public class Class1
    {
        public void Test()
        {
            Lua lua = new Lua();
            dynamic g = lua.CreateEnvironment();
            g.Henkie = new Action(PrintHenkie);
        }

        public void PrintHenkie()
        {
            Console.WriteLine("Henkie");
        }
    }
}
