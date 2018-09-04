using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Honeymustard
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TestController
    {
        protected IPathService Paths;

        public TestController(IPathService paths)
        {
            Paths = paths;
        }

        // GET api/test/fetch
        [HttpGet("fetch")]
        public string Fetch()
        {
            return HTTP.Fetch(new Uri("https://www.nrk.no"));
        }

        // GET api/test/parse
        [HttpGet("parse")]
        public string Parse()
        {
            return File.ReadAllText(Path.Combine(Paths.GetDataPath(), "published-today.html"));
        }
    }
}