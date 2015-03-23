using System.Collections.Generic;
using Autofac;
using Nancy;
using Obsession.Core;

namespace Obsession.Service.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule(IStateManager manager)
        {
            Get["/values"] = x =>
                {
                    string view = "";
                    
                    foreach (var moduleState in manager.GetActualState())
                    {
                        foreach (var value in moduleState.Value as IDictionary<string, object>)
                        {
                            view += string.Format("<p>{0}.{1} = {2}</p>", moduleState.Key, value.Key, value.Value.ToString());
                        }
                    }
                    return view;
                };
        }
    }
}