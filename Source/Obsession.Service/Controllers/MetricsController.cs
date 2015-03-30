using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Nest;
using Obsession.Core;

namespace Obsession.Service.Controllers
{
    public class MetricsController : ApiController
    {
        private readonly IElasticClient _client;

        public MetricsController(IElasticClient client)
        {
            _client = client;
        }

        [HttpGet]
        [Route("api/metrics/get/{metric}/{start}/{stop}/{step}")]
        public IEnumerable<HistoValue> Get(string metric, DateTime start, DateTime stop, long step)
        {
            var subAggKey = "avg";

            var instance = metric.Split('.')[0];
            var m = metric.Split('.')[1];
            var result = _client
                .Search<StateValues>(
                    d =>
                    d.Aggregations(a =>
                                   a.DateHistogram("histo", h =>
                                                            h.Field(
                                                                "@timestamp")
                                                             .Interval("1m")
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
                if( subAggValue != null && (subAggValue as ValueMetric).Value.HasValue)
                        r.Add(i.Date, new HistoValue()
                            {
                                DateTime = i.Date,
                                Value = (i.Aggregations.Single(a => a.Key == subAggKey).Value as ValueMetric).Value.Value
                            });
            }

            var time = start;
            do
            {
                if (!r.ContainsKey(time))
                    r.Add(time, new HistoValue {DateTime = time, Value = 0});
                time = time.AddHours(1);
            } while (time <= stop);

            return r.Values.OrderBy(v => v.DateTime);
        }

        public class HistoValue
        {
            public DateTime DateTime { get; set; }
            public double Value { get; set; }
        }
    }
}
