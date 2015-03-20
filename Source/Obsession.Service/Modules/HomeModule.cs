using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Nancy;
using Nancy.Owin;
using Nest;
using Obsession.Core;
using Obsession.Service.Configuration;

namespace Obsession.Service
{
    public class HomeModule : NancyModule
    {
        public HomeModule(ElasticClient client)
        {
            Get["/"] = x =>
                {
                    string view = "";

//                    foreach (var provider in providers)
//                    {
//                        foreach (var param in provider.GetParams())
//                        {
//                            view += string.Format("<p>{0} = {1}</p>", param.Name, param.ToString());
//                        }
//                    }
                    return view;
                };
        }
    }
}