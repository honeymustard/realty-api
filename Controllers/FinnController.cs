using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Honeymustard
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FinnController : Controller
    {
        public Uri UriPublishedToday = new Uri("https://www.finn.no/realestate/homes/search.html?location=0.20061&published=1&rows=9999");

        // GET api/finn/fetch
        [HttpGet("fetch")]
        public string Fetch()
        {
            return HTTP.Fetch(new Uri("https://www.nrk.no"));
        }
    }
}