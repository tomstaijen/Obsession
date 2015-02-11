using Autofac;
using P1Reader;

namespace Obsession.Service.Configuration
{
    class P1Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<P1ReaderService>();
        }
    }
}
