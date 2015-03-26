using Autofac;
using Obsession.Core;
using Obsession.Core.Effectors;
using Obsession.Core.Persistence;
using Obsession.Core.Rules;
using Obsession.Internal.LuaEngine;

namespace Obsession.Service.AutofacModules.Obsession
{
    public class ObsessionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // stores
            builder.RegisterType<InjectableConfigStore>().As<IStore<Configuration>>().InstancePerLifetimeScope();
            builder.RegisterType<StaticRuleStore>().As<IStore<Rule>>();

            // processing
            builder.RegisterType<StateManager>().As<IStateManager>().SingleInstance();
            builder.RegisterType<MoonEngine>().As<IEngine>();
            builder.RegisterType<EngineContextProvider>().As<IEngineContextProvider>();
            builder.RegisterType<ModuleFactory>().As<IModuleFactory>();

            // actors
            builder.RegisterType<PluginManager>();
            builder.RegisterType<PluginController>();
            builder.RegisterType<EventProcessor>();
        }
    }
}
