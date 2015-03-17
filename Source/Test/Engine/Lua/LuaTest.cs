using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Obsession.Internal.LuaEngine;

namespace Test.Engine.Lua
{
    [TestFixture]
    public class LuaTest
    {
        [Test]
        public void MoonTest()
        {
            var engine = new MoonEngine();
            engine.Register("name", "Pietje");
            engine.Register("notify", (Action<string>)Notify);
            Console.WriteLine(engine.Run(@"
notify(name);
return 1+2;
            "));
        }

        [Test]
        public void Test()
        {
            var lua = new NeoLuaEngine();
            lua.Test();
        }

        public void Notify(string name)
        {
            Console.WriteLine("Hello {0}", name);
        }
    }
}
