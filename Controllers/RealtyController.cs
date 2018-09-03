using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Honeymustard.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RealtyController : Controller
    {
        // GET api/realty
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/realty/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/realty
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/realty/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/realty/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}