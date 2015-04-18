using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Autofac;
using Autofac.Integration.WebApi;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Conventions;
using Nancy.Diagnostics;
using Nancy.Helpers;
using Newtonsoft.Json.Serialization;
using Obsession.Service.AutofacModules;
using Obsession.Service.AutofacModules.Obsession;
using Obsession.Service.ReactStuff;
using React;

namespace Obsession.Service
{
    public class ForceCamelCaseAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings currentConfiguration, HttpControllerDescriptor currentDescriptor)
        {
            var currentFormatter = currentConfiguration.Formatters.OfType<JsonMediaTypeFormatter>().Single();
            //remove the current formatter
            currentConfiguration.Formatters.Remove(currentFormatter);

            var camelFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = { ContractResolver = new CamelCasePropertyNamesContractResolver() }
            };
            //add the camel case formatter
            currentConfiguration.Formatters.Add(camelFormatter);
        }
    }

    public class Bootstrapper : AutofacNancyBootstrapper
    {
        private static object _containerLock = new object();
        private static IContainer _container;

        public static IContainer GetContainer()
        {
            if (_container == null)
            {
                lock (_containerLock)
                {
                    if (_container == null)
                    {
                        var builder = new ContainerBuilder();

                        builder.RegisterModule<ObsessionModule>();
                        builder.RegisterModule<AkkaModule>();
                        builder.RegisterModule<ElasticModule>();
                        builder.RegisterModule<MongoModule>();
                        builder.RegisterModule<P1Module>();
                        builder.RegisterModule<InComfortModule>();
                        builder.RegisterModule<YahooWeatherModule>();
                        builder.RegisterModule<ReactModule>();
                        builder.RegisterModule<NmaModule>();

                        builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

                        _container = builder.Build();
                    }
                }
            }
            return _container;
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = @"blerp"}; }
        }

        protected override ILifetimeScope GetApplicationContainer()
        {
            return GetContainer();
        }

        protected override ILifetimeScope CreateRequestContainer()
        {
            return GetContainer().BeginLifetimeScope();
        }

        protected override ILifetimeScope CreateRequestContainer(NancyContext context)
        {
            return GetContainer().BeginLifetimeScope();
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("content"));
            
            conventions.StaticContentsConventions.Add((ctx, s) =>
            {
                if (ctx.Request.Path.EndsWith(".jsx"))
                {
                    var react = GetApplicationContainer().Resolve<IReactEnvironment>();
                    return react.JsxTransformer.TransformJsxFile("~" + ctx.Request.Path);
                }
                return null;
            });
        }
    }
}
