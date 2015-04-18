using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Autofac.Features.OwnedInstances;

namespace Obsession.Akka
{
    public class MetricsSupplier : TypedActor, IHandle<FetchMetrics>
    {
        public void Handle(FetchMetrics message)
        {
            Sender.Tell(new Metrics()
                {
                    Values = new Dictionary<DateTime, double>()
                }, Self);
        }
    }

    public class FetchMetrics
    {
        public string Metric { get; set; }
    }

    public class Metrics
    {
        public Dictionary<DateTime, double> Values { get; set; }
    }
}
