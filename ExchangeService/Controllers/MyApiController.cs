using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ExchangeService.Controllers
{
    public class MyApiController : ApiController
    {
        // GET: api/Api
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Api/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Api
        public void Post([FromBody]string value)
        {

    }

        // PUT: api/Api/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Api/5
        public void Delete(int id)
        {
        }
    }
}
