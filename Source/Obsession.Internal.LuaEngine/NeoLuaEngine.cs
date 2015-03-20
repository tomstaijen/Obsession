using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using Neo.IronLua;

namespace Obsession.Internal.LuaEngine
{
    public static class ObjectToDictionaryHelper
    {
        public static IDictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        public static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
                AddPropertyToDictionary<T>(property, source, dictionary);
            return dictionary;
        }

        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
        {
            object value = property.GetValue(source);
            if (IsOfType<T>(value))
                dictionary.Add(property.Name, (T)value);
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
        }
    }


    public interface IEngine
    {
        void RegisterData(string method, object data);
        void RegisterFunction<T>(string method, Action<T> action);
        object Run(string script);
    }

    public class MoonEngine : IEngine
    {
        private Script _script;

        public MoonEngine()
        {
            _script = new Script();
        }

        public void RegisterData(string method, object value)
        {
            var valAsDict = value.ToDictionary();
            _script.Globals[method] = valAsDict;
        }

        public void RegisterFunction<T>(string name, Action<T> a)
        {
            _script.Globals[name] = a;
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
            return dynValue.ToObject<T>();
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
