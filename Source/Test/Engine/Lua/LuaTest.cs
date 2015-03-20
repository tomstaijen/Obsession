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
            engine.RegisterData("name", "Pietje");
            engine.RegisterFunction("notify", (Action<string>)Notify);
            // can override
            engine.RegisterFunction("notify", (Action<string>)Notify2);

            Console.WriteLine(engine.Run(@"
a = 'tom';
notify(name .. a);
return 1+2;
            "));

            string x = engine.Retrieve<string>("return tom;");
            Console.WriteLine(x);
        }

        [Test]
        public void MoonTestDictionary()
        {
            var o = new MyObject()
                {
                    DeString = "MijnString"
                };
            //o.WeerEenDictionary.Add("key", "fiets");

            var dictionary = new Dictionary<string, object>();
            dictionary.Add("o", o);

            var engine = new MoonEngine();
            engine.RegisterData("hallo", o);
            engine.RegisterFunction("notify", (Action<string>)Notify);

            Console.WriteLine(engine.Run(@"
                notify(hallo.DeString);
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

    public class MyObject
    {
        public string DeString { get; set; }

        //public Dictionary<string, object> WeerEenDictionary { get; set; }
    }
}
