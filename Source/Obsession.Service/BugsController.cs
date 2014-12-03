using System.Web.Http;

namespace Obsession.Service
{
    public class BugsController : ApiController
    {
        [HttpGet]
        [Route("api/blerp")]
        public string Get()
        {
            return "Hallo";
        }
    }
}