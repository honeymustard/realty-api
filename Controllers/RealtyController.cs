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
        protected IBrowser Browser;
        protected IRepository<RealtyDocument> Repository;

        public RealtyController(IBrowser browser, IRepository<RealtyDocument> repository)
        {
            Browser = browser;
            Repository = repository;
        }

        // GET api/realty/parse/today
        [HttpGet("parse/today")]
        public IActionResult Today()
        {
            var uri = new Uri("https://www.finn.no/realestate/homes/search.html?location=0.20061&published=1&rows=9999");
            var container = "<div class=\"unit flex align-items-stretch result-item\">";

            var parser = new Parser(Browser.Fetch(uri));
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
    }
}