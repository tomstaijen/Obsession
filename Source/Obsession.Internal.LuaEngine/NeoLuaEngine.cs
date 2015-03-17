using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using Neo.IronLua;

namespace Obsession.Internal.LuaEngine
{

    public interface IEngine
    {
        void Register(string method, object callback);
        object Run(string script);
    }

    public class MoonEngine : IEngine
    {
        private Script _script;

        public MoonEngine()
        {
            _script = new Script();
        }

        public void Register(string method, object value)
        {
            _script.Globals[method] = value;
        }

        public object Run(string script)
        {
            return _script.DoString(script);
        }

        public dynamic Get(string fact)
        {
            return _script.Globals[fact];
        }

        public T Retrieve<T>(string expr)
        {
            var dynValue = _script.DoString(expr);
        }
    }

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
