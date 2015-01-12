using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Conventions;
using Obsession.Service.Configuration;
using Obsession.Service.ReactStuff;
using React;

namespace Obsession.Service
{
    public class Bootstrapper : AutofacNancyBootstrapper 
    {
        protected override ILifetimeScope GetApplicationContainer()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<ElasticModule>();
            builder.RegisterModule<MongoModule>();
            builder.RegisterModule<P1Module>();
            builder.RegisterModule<InComfortModule>();
            builder.RegisterModule<YahooWeatherModule>();
            builder.RegisterModule<ReactModule>();

            return builder.Build();
        }
        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("assets", @"content/assets"));
            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Scripts", @"content/scripts"));
        }

    }
}
