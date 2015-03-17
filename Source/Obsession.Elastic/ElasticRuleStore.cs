using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Obsession.Core;
using Obsession.Core.Rules;

namespace Obsession.Elastic
{
    public class ElasticRuleStore : IRuleStore
    {
        private readonly IElasticClient _client;

        public ElasticRuleStore(IElasticClient client)
        {
            _client = client;
        }

        public IEnumerable<Rule> GetRules()
        {
            return _client.Search<Rule>(q => q).Documents;
        }
    }
}
