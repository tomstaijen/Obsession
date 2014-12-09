using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Nancy.Bootstrappers.Autofac;
using Nancy.Conventions;
using Obsession.Service.Configuration;

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

            return builder.Build();
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("assets", @"content/assets"));
        }
    }
}
