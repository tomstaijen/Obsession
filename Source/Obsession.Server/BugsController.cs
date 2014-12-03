using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace NancyOwinApi
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