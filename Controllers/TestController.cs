using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Honeymustard
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        protected IPathService Paths;
        protected IUtilityService Utilities;
        protected IHTTPService HTTP;
        protected RealtyRepository Repository;

        public TestController(
            IPathService paths,
            IUtilityService utilities,
            IHTTPService http,
            RealtyRepository repository)
        {
            Paths = paths;
            Utilities = utilities;
            HTTP = http;
            Repository = repository;
        }

        // GET api/test/fetch
        [HttpGet("fetch")]
        public string Fetch()
        {
            return HTTP.Fetch(new Uri("https://www.nrk.no"));
        }

        // GET api/test/show
        [HttpGet("show/{name}")]
        public string Show(string name)
        {
            return Utilities.ReadFile(Paths.GetDataPath(), $"{name}.html");
        }

        // GET api/test/parse/today
        [HttpGet("parse/today")]
        public IActionResult Parse()
        {
            var file = Utilities.ReadFile(Paths.GetDataPath(), "today.html");
            var container = "<div class=\"unit flex align-items-stretch result-item\">";

            var parser = new Parser(file);
            var indices = parser.FindIndices(container);
            var partitions = parser.Partition(indices);
            var chunks = parser.Chunk(partitions);
            var models = chunks.Select(e => new RealtyParser().Parse(e));
            var documents = models.Select(e => AutoMapper.Mapper.Map<RealtyDocument>(e));

            var todays = Repository.FindAny(RealtyRepository.FilterToday);
            var realties = documents.Where(e => !todays.Any(item => item.RealtyId == e.RealtyId));

            return Json(new {
                results = documents.Count(),
                inserted = realties.Count(),
                ignored = todays.Count(),
            });
        }
    }
}