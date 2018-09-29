using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Honeymustard.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RealtyController : Controller
    {
        protected IEnvironment Environment;
        protected IBrowser Browser;
        protected IRepository<RealtyDocument> Repository;

        public RealtyController(
            IEnvironment environment,
            IBrowser browser,
            IRepository<RealtyDocument> repository)
        {
            Environment = environment;
            Browser = browser;
            Repository = repository;
        }

        [HttpGet("parse/today")]
        public IActionResult Today()
        {
            var uri = new Uri("https://www.finn.no/realestate/homes/search.html?location=0.20061&published=1&rows=9999");
            var container = "<div class=\"unit flex align-items-stretch result-item\">";
            var content = Browser.Fetch(uri);

            var parser = new TextParser(content)
                .Strip(TextParser.WhiteSpace)
                .Strip(TextParser.ScriptTags);

            var indices = parser.FindIndices(container);
            var partitions = parser.Partition(indices);

            var chunks = parser.Chunk(partitions)
                .Where(e => !new Regex(@"id=""promoted-[0-9]{3,16}""").Match(e).Success);
            var models = chunks.Select(e => new RealtyParser().Parse(e));
            var documents = models.Select(e => AutoMapper.Mapper.Map<RealtyDocument>(e));
            var todays = Repository.FindAny(RealtyRepository.FilterToday);
            var newRealties = documents.Where(e => !todays.Any(item => item.RealtyId == e.RealtyId));
            var duplicates = documents.Where(e => todays.Any(item => item.RealtyId == e.RealtyId));

            if (newRealties.Count() > 0)
            {
                Repository.InsertMany(newRealties);
            }

            return Json(new {
                total = documents.Count(),
                newRealties = newRealties.Count(),
                duplicateRealties = duplicates.Count(),
                newToday = todays.Count() + newRealties.Count(),
                documents = documents,
            });
        }
    }
}