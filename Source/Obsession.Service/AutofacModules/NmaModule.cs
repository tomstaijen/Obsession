using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NotifyMyAndroid;
using Obsession.Core;
using Obsession.Core.Extensions;

namespace Obsession.Service.AutofacModules
{
    public class NmaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NmaServiceModule>().Named<IServiceModule>("nma");
            builder.RegisterInstance(
                new Configuration("nma", "nma") 
                    {
                        Poll = false
                    }
                    .WithValue(NmaServiceModule.ApiKeyKey, "9bdad31063771a8b755088ed25ac4aa00034aad44442d0a3")
                    .WithValue(NmaServiceModule.ApplicationKey, "Obsession")
                    .WithValue(NmaServiceModule.Message, "Notification")
                );
        }
    }
}
