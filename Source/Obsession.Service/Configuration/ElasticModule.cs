using System;
using Autofac;
using Nest;

namespace Obsession.Service.Configuration
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
        }
    }
}
