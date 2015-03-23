using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using Obsession.Core;
using Obsession.Core.Helpers;

namespace Obsession.Internal.LuaEngine
{
    public class MoonEngine : IEngine
    {
        private Script _script;

        public MoonEngine()
        {
            _script = new Script();
        }

        public void RegisterContext(string key, object value)
        {
            _script.Globals[key] = value;
        }

        public void RegisterContext(IDictionary<string, object> context)
        {
            foreach (var x in context)
            {
                RegisterContext(x.Key, x.Value);
            }
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
}