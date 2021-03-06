using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Honeymustard
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        protected IEnvironment Environment;
        protected IUtilities Utilities;
        protected IBrowser Browser;
        protected IRealtyRepository Repository;
        protected ILogger Logger;

        public TestController(
            IEnvironment environment,
            IUtilities utilities,
            IBrowser browser,
            IRealtyRepository repository,
            ILogger<TestController> logger)
        {
            Environment = environment;
            Utilities = utilities;
            Browser = browser;
            Repository = repository;
            Logger = logger;
        }

        [Authorize]
        [HttpGet("secrets")]
        public IActionResult Secrets()
        {
            return Ok(new { secrets = "yes" });
        }

        [HttpGet("non-secrets")]
        public IActionResult NonSecrets()
        {
            return Ok(new { secrets = "nope" });
        }

        [HttpGet("fetch")]
        public string Fetch()
        {
            return Browser.Fetch(new Uri("https://honeymustard.io"));
        }

        [HttpGet("show/{name}")]
        public string Show(string name)
        {
            return Utilities.ReadFile(Environment.GetDataPath(), $"{name}.html");
        }

        [HttpGet("parse/today")]
        public IActionResult Parse()
        {
            var file = Utilities.ReadFile(Environment.GetDataPath(), "today-180927.html");
            var container = "<div class=\"unit flex align-items-stretch result-item\">";

            var parser = new TextParser(file)
                .Strip(TextParser.WhiteSpace)
                .Strip(TextParser.ScriptTags);

            var indices = parser.FindIndices(container);
            var partitions = parser.Partition(indices);

            var chunks = parser.Chunk(partitions)
                .Where(e => !new Regex(@"id=""promoted-[0-9]{3,16}""").Match(e).Success);

            var models = chunks.Aggregate(new List<RealtyModel>(), (accumulator, chunk) => {
                var realty = new RealtyParser().Parse(chunk);

                if (TryValidateModel(realty))
                {
                    accumulator.Add(realty);
                }
                else
                {
                    Logger.LogError("Realty did not pass validation");
                    Logger.LogError(chunk);
                }

                return accumulator;
            });

            var documents = models.Select(e => AutoMapper.Mapper.Map<RealtyDocument>(e));

            var todays = Repository.FindAny(RealtyRepository.FilterToday).ToList();
            var newRealties = documents.Where(e => !todays.Any(item => item.RealtyId == e.RealtyId));
            var duplicates = documents.Where(e => todays.Any(item => item.RealtyId == e.RealtyId));

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