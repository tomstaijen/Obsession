using Autofac;
using JavaScriptEngineSwitcher.Msie;
using JavaScriptEngineSwitcher.Msie.Configuration;
using Nancy;
using Obsession.Service.ReactStuff;
using React;

namespace Obsession.Service.AutofacModules
{
    public class ReactModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomRootPathProvider>().As<IRootPathProvider>();

            builder.RegisterInstance(ReactSiteConfiguration.Configuration).As<IReactSiteConfiguration>();
            
            builder.RegisterType<FileCacheHash>().As<IFileCacheHash>().InstancePerLifetimeScope();

            builder.RegisterType<JavaScriptEngineFactory>().As<IJavaScriptEngineFactory>().SingleInstance();

            builder.RegisterType<ReactEnvironment>().As<IReactEnvironment>().InstancePerLifetimeScope();

            // JavaScript engines
            builder.RegisterInstance(new JavaScriptEngineFactory.Registration
            {
                Factory = () => new MsieJsEngine(new MsieConfiguration { EngineMode = JsEngineMode.ChakraActiveScript }),
                Priority = 20
            });
            
            builder.RegisterInstance(new JavaScriptEngineFactory.Registration
            {
                Factory = () => new MsieJsEngine(new MsieConfiguration { EngineMode = JsEngineMode.Classic }),
                Priority = 30
            });

            builder.RegisterType<NancyReactFileSystem>().As<IFileSystem>().InstancePerLifetimeScope();
            builder.RegisterType<NullReactCache>().As<ICache>();
        }
    }
}
