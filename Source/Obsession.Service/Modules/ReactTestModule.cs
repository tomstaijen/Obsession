﻿using Nancy;
using React;

namespace Obsession.Service.Modules
{
    public class ReactTestModule : NancyModule
    {
        public ReactTestModule(IReactEnvironment react)
        {
            Get[@"(?<imagepath>.*)(?<imagename>.*.jsx)"] = _ =>
            {
                var s =  react.JsxTransformer.TransformJsxFile("~/views/hello.jsx");
                return s;
//                react.Execute(hellojs);
//                var component = react.CreateComponent("HelloWorld", new Person { firstName = "Prabir", lastName = "Shrestha" });
                /// var component = react.CreateComponent("HelloWorld", new { firstName = "Prabir", lastName = "Shrestha" });
//                var html = component.RenderHtml();
//                return html;
            };            
        }
    }

    class Person
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
}
