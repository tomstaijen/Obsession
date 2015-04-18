using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.Core;

namespace Obsession.Core.Extensions
{
    public static class ExtensionsToActorContext
    {
        public static IActorRef Resolve<T>(this IUntypedActorContext context) where T : ActorBase
        {
            var resolver = context.Props.Arguments.OfType<IDependencyResolver>().SingleOrDefault();
            if( resolver == null )
                throw new InvalidProgramException("DI resolver expected");

            return context.ActorOf(resolver.Create<T>());
        }
    }
}
