using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo.IronLua;

namespace Obsession.Internal.LuaEngine
{
    public class NeoLuaEngine
    {
        public void Test()
        {
            Lua lua = new Lua();
            var g = lua.CreateEnvironment();
            g.DefineMethod("Henkie", new Action(PrintHenkie));
            g.DoChunk("Henkie()");
        }

        public void PrintHenkie()
        {
            Console.WriteLine("Henkie");
        }
    }
}
