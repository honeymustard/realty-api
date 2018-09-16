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
        protected RealtyRepository Repository;

        public RealtyController(RealtyRepository repository)
        {
            Repository = repository;
        }

        // GET api/realty/parse/today
        [HttpGet("parse/today")]
        public IActionResult Today()
        {
            var uri = new Uri("https://www.finn.no/realestate/homes/search.html?location=0.20061&published=1&rows=9999");
            var container = "<div class=\"unit flex align-items-stretch result-item\">";

            var parser = new Parser(HTTP.Fetch(uri));
            var indices = parser.FindIndices(container);
            var partitions = parser.Partition(indices);
            var chunks = parser.Chunk(partitions);
            var models = chunks.Select(e => new RealtyParser().Parse(e));
            var documents = models.Select(e => AutoMapper.Mapper.Map<RealtyDocument>(e));

            var todays = Repository.FindAny(RealtyRepository.FilterToday);
            var realties = documents.Where(e => !todays.Any(item => item.RealtyId == e.RealtyId));

            if (realties.Count() > 0)
            {
                Repository.InsertMany(realties);
            }

            return Json(new {
                results = documents.Count(),
                inserted = realties.Count(),
                ignored = todays.Count(),
            });
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