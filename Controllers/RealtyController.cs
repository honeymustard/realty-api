using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Honeymustard.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RealtyController : Controller
    {
        protected IEnvironment Environment;
        protected IMemoryCache Cache;
        protected IBrowser Browser;
        protected IRealtyRepository Repository;
        protected ILogger<RealtyController> Logger;

        public RealtyController(
            IEnvironment environment,
            IMemoryCache cache,
            IBrowser browser,
            IRealtyRepository repository,
            ILogger<RealtyController> logger
        )
        {
            Environment = environment;
            Cache = cache;
            Browser = browser;
            Repository = repository;
            Logger = logger;
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

            if (newRealties.Count() > 0)
            {
                Repository.InsertMany(newRealties);
            }

            return Json(new {
                total = documents.Count(),
                newRealties = newRealties.Count(),
                duplicateRealties = duplicates.Count(),
                newToday = todays.Count() + newRealties.Count(),
            });
        }

        [HttpGet("datum")]
        public IActionResult Datum(DateTime from)
        {
            List<Datum> datum;

            var key = $"realty/datum/{from.ToString("yyyy-MM-dd")}";

            if (!Cache.TryGetValue(key, out datum))
            {
                var map = new Dictionary<string, Datum>();
                var builder = MongoDB.Driver.Builders<RealtyDocument>.Filter;
                var filter = builder.Gte("Added", from) & builder.Lt("Added", DateTime.Today);

                foreach (var item in Repository.FindAny(filter))
                {
                    var day = item.Added.ToString("yyyy-MM-dd");

                    if (map.ContainsKey(day))
                    {
                        map[day].Value++;
                    }
                    else
                    {
                        map[day] = new Datum {
                            Date = DateTime.Parse(day),
                            Value = 0,
                        };
                    }
                }

                datum = map.Values.ToList();

                Cache.Set(key, datum, new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(1)));
            }

            return Json(datum);
        }
    }
}