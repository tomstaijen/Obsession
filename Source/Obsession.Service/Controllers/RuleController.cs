using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Obsession.Core.Persistence;
using Obsession.Core.Rules;

namespace Obsession.Service.Controllers
{
    public class RuleController : ApiController
    {
        private readonly IStore<Rule> _ruleStore;

        public RuleController(IStore<Rule> ruleStore)
        {
            _ruleStore = ruleStore;
        }

        [HttpGet]
        [Route("api/get/rules")]
        public IEnumerable<Rule> GetRules()
        {
            return _ruleStore.GetThem();
        }
    }
}
