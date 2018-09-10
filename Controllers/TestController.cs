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
        protected RealtyRepository Repository;

        public TestController(IPathService paths, RealtyRepository repository)
        {
            Paths = paths;
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
            return Utils.ReadFile(Paths.GetDataPath(), $"{name}.html");
        }

        // GET api/test/parse/today
        [HttpGet("parse/today")]
        public IActionResult Parse()
        {
            var file = Utils.ReadFile(Paths.GetDataPath(), "today.html");

            var container = "<div class=\"unit flex align-items-stretch result-item\">";

            var parser = new Parser(file);
            var indices = parser.FindIndices(container);
            var partitions = parser.Partition(indices);
            var chunks = parser.Chunk(partitions);
            var models = chunks.Select(e => new RealtyParser().Parse(e));
            var documents = models.Select(e => AutoMapper.Mapper.Map<RealtyDocument>(e));

            return Json(documents);
        }
    }
}