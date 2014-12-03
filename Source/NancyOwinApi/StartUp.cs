﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using Nancy;
using Owin;

namespace NancyOwinApi
{
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy(options =>
              options.PerformPassThrough = context =>
                  context.Response.StatusCode == HttpStatusCode.NotFound);

            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("bugs", "api/{Controller}");
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            app.UseWebApi(config);
        }
    }
}