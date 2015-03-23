using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Obsession.Core;
using Obsession.Core.Persistence;
using Obsession.Core.Rules;

namespace Obsession.Elastic
{
    public class ElasticPersister : IPersister
    {
        private readonly IElasticClient _client;

        public ElasticPersister(IElasticClient client)
        {
            _client = client;
        }

        public void Put<T>(long id, T o) where T : class
        {
            _client.Index<T>(o, i => i.Id(id));
        }

        public void Put<T>(T o) where T : class
        {
            _client.Index(o);
        }

    }

    public class ElasticStore<T> : IStore<T> where T : class
    {
        private readonly IElasticClient _client;

        public ElasticStore(IElasticClient client)
        {
            _client = client;
        }

        public IEnumerable<T> GetThem()
        {
            return _client.Search<T>(q => q).Documents;
        }
    }

}
