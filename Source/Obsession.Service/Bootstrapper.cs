using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Conventions;
using Nancy.Diagnostics;
using Nancy.Helpers;
using Obsession.Service.AutofacModules;
using Obsession.Service.AutofacModules.Obsession;
using Obsession.Service.ReactStuff;
using React;

namespace Obsession.Service
{
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

//        protected override void ConfigureConventions(NancyConventions conventions)
//        {
//            base.ConfigureConventions(conventions);
//
//            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("content"));
//            
//            conventions.StaticContentsConventions.Add((ctx, s) =>
//            {
//                if (ctx.Request.Path.EndsWith(".jsx"))
//                {
//                    var react = GetApplicationContainer().Resolve<IReactEnvironment>();
//                    return react.JsxTransformer.TransformJsxFile("~" + ctx.Request.Path);
//                }
//                return HttpStatusCode.NotFound;
//            });
//
//        }

        private static string GetSafeFileName(string path)
        {
            try
            {
                return Path.GetFileName(path);
            }
            catch (Exception)
            {
            }

            return null;
        }


        private static string GetPathWithoutFilename(string fileName, string path)
        {
            var pathWithoutFileName =
                path.Replace(fileName, string.Empty);

            if (pathWithoutFileName[0] == '/')
            {
                pathWithoutFileName = pathWithoutFileName.Substring(1);
            }

            return (pathWithoutFileName.Equals("/")) ?
                pathWithoutFileName :
                pathWithoutFileName.TrimEnd(new[] { '/' });
        }
    }
}
