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
    public class MoonScriptLuaTest
    {


        [Test]
        public void CanRunScript()
        {
            
        }

        [Test]
        public void MoonTest()
        {
            var engine = new MoonEngine();
            engine.Register("name", "Pietje");
            engine.Register("notify", (Action<string>)Notify);
            // can override
            engine.Register("notify", (Action<string>)Notify2);

            Console.WriteLine(engine.Run(@"
a = 'tom';
notify(name .. a);
return 1+2;
            "));

            string x = engine.Retrieve<string>("return tom;");
            Console.WriteLine(x);
        }

        public void Notify(string name)
        {
            Console.WriteLine("Hello {0}", name);
        }

        public void Notify2(string name)
        {
            Console.WriteLine("Blerp {0}", name);
        }
    }
}
