using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Akka.Actor;
using Nest;
using Obsession.Akka;
using Obsession.Core;

namespace Obsession.Service.Controllers
{
    [ForceCamelCase]
    public class MetricsController : ApiController
    {
        private readonly IElasticClient _client;
        private readonly ActorSystem _actorSystem;

        public MetricsController(IElasticClient client, ActorSystem actorSystem)
        {
            _client = client;
            _actorSystem = actorSystem;
        }

        [HttpGet]
        [Route("api/metrics/hour/{metric}/{fill}")]
        public IEnumerable<HistoValue> Hour(string metric, bool fill)
        {
            var now = DateTime.Now;
            var prev = now.AddHours(-1);

            return Query(metric, prev, now, "30s", fill ? new TimeSpan(0, 0, 30) : (TimeSpan?)null);
        }

        [HttpGet]
        [Route("api/metrics/day/{metric}/{fill}")]
        public IEnumerable<HistoValue> Day(string metric, bool fill)
        {
            var actor = _actorSystem.ActorOf<MetricsSupplier>();
            var result = actor.Ask<Metrics>(new FetchMetrics());


            var x = result.Result.Values.Select(kvp => new HistoValue(kvp.Key, kvp.Value));
            return x;

            var now = DateTime.Now;
            var prev = now.AddDays(-1);

            return Query(metric, prev, now, "1m", fill ? new TimeSpan(0, 10, 0) : (TimeSpan?)null);
        }

        

        public IEnumerable<HistoValue> Query(string metric, DateTime start, DateTime stop, string step, TimeSpan? fillInterval = null)
        {
            var subAggKey = "avg";

            var instance = metric.Split('.')[0];
            var m = metric.Split('.')[1];
            var result = _client
                .Search<StateValues>(
                    d => //d.Query(qc => qc.Range(sel => sel.OnField("@timestamp").GreaterOrEquals(start.ToUniversalTime()).LowerOrEquals(stop.ToUniversalTime())))
                    d.Aggregations(a =>
                                   a.DateHistogram("histo", h =>
                                                            h.Field(
                                                                "@timestamp")
                                                             .Interval(step)
                                                             .Aggregations(
                                                                 sa1 =>
                                                                 sa1.Average(
                                                                     subAggKey,
                                                                     sa1s =>
                                                                     sa1s.Field
                                                                         ("values." +
                                                                          m)))
                                       )));

            var r = new Dictionary<DateTime, HistoValue>();

            foreach (var i in result.Aggs.Histogram("histo").Items)
            {
                var subAggValue = i.Aggregations.Single(a => a.Key == subAggKey).Value;
                if (subAggValue != null && (subAggValue as ValueMetric).Value.HasValue && i.Date >= start.ToUniversalTime())
                    r.Add(i.Date, new HistoValue(i.Date.ToLocalTime(), (i.Aggregations.Single(a => a.Key == subAggKey).Value as ValueMetric).Value.Value));
            }

            if (fillInterval.HasValue)
            {
                var time = start;
                do
                {
                    if (!r.ContainsKey(time))
                        r.Add(time, new HistoValue(time, 0));
                    
                    time = time.Add(fillInterval.Value);

                } 
                while (time <= stop);
            }

            return r.Values.OrderBy(d => d.Timestamp).ToList();
        }

        public class HistoValue
        {
            public HistoValue(DateTime timestamp, double value)
            {
                Timestamp = timestamp;
                Value = value;
            }

            public static DateTime Epoch = new DateTime(1970, 1, 1);
            public static double ToTicks(DateTime dt)
            {
                return (dt - Epoch).TotalSeconds;
            }

            public DateTime Timestamp { get; set; }
            //public double Ticks { get { return ToTicks(Timestamp); } }
            public double Value { get; set; }
        }
    }
}
