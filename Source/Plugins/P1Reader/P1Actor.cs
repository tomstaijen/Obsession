using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace P1Reader
{
    public class P1Request
    {
    }

    public class P1Actor : ReceiveActor
    {
        public P1Actor()
        {
            Receive<P1Request>(Handle);
        }

        public bool Handle(P1Request request)
        {
            Context.System.EventStream.Publish(new P1Envelope());
            return true;
        }
    }
}
