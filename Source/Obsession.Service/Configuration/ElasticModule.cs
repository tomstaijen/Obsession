using System;
using Autofac;
using Nest;
using Obsession.Core.Persistence;
using Obsession.Elastic;

namespace Obsession.Service.AutofacModules
{
    public class ElasticModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(
                node,
                defaultIndex: "obsession"
                );

            builder.Register(c => new ElasticClient(settings)).As<IElasticClient>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof (ElasticStore<>)).As(typeof (IStore<>));
            builder.RegisterType<ElasticPersister>().As<IPersister>();
        }
    }
}
