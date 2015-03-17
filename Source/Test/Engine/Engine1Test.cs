using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Newtonsoft.Json;
using Obsession.Core.Rules.Engine1;

namespace ConsoleApplication1
{
    [TestFixture]
    public class Program
    {
        [Test]
        public void RecursiveUseOfEngine()
        {
            var func = FunctionFactory.Create<Person, bool>("Ages(\"Value == 1\").Any()");
            var person = new Person() { Age = 31 };
            Console.WriteLine(func(person));
        }

        [Test]
        public void ParseBlock()
        {
            var func = FunctionFactory.Create<Person, bool>("WriteAge().HasAge(31)");
            var person = new Person() { Age = 31 };
            Console.WriteLine(func(person));
        }
    }

    public class Person
    {
        public int Age { get; set; }

        public int Ages(bool a)
        {
            if( a )
                return 100;
            return 200;
        }

        public bool HasAge(int age)
        {
            return Age == age;
        }

        public Person WriteAge()
        {
            Console.WriteLine(Age);
            return this;
        }

        public int[] Ages2(Predicate<IntContainer> predicate)
        {
            return new [] {new IntContainer() {Value = 1}}.Where(i => predicate(i)).Select(v => v.Value).ToArray();
        }

        public int[] Ages(string predicate)
        {
            var func = FunctionFactory.Create<IntContainer, bool>(predicate);
            return new [] { new IntContainer() { Value = 1 } }.Where(func).Select(v => v.Value).ToArray();
        }
    }

    public class IntContainer
    {
        public int Value { get; set; }
    }


}
