using System;
using System.Text.RegularExpressions;

namespace Honeymustard
{
    public class RealtyParser : IParser<RealtyModel>
    {
        public RealtyModel Parse(string chunk)
        {
            Match match = null;
            match = Regex.Match(chunk, @"data-finnkode=\""(\d+)\""");
            var realtyId = match.Groups[1].Value;

            match = Regex.Match(chunk, @"<img src=\""(.*?)\""");
            var imageUri = match.Groups[1].Value;

            match = Regex.Match(chunk, @"data-automation-id=\""\w*?\"">(.*?)</div>");
            var address = match.Groups[1].Value;

            match = Regex.Match(chunk, @"<h3 .*?>(.*)</h3>");
            var description = match.Groups[1].Value;

            match = Regex.Match(chunk, @"<span .*?>([0-9]{1,5}).*?</span>");
            var squareMeters = match.Groups[1].Value;

            match = Regex.Match(chunk, @"<span .*?>([0-9 ]{3,20}),-</span>");
            var price = match.Groups[1].Value.Replace(" ", "");

            match = Regex.Match(chunk, @"Fellesgjeld: ([0-9 ]{3,15}),-");
            var sharedDept = match.Groups[1].Value.Replace(" ", "");

            match = Regex.Match(chunk, @"Fellesutg\.?: ([0-9 ]{3,15}),-");
            var sharedExpenses = match.Groups[1].Value.Replace(" ", "");

            return new RealtyModel {
                RealtyId = realtyId,
                ImageUri = imageUri,
                Address = address,
                Description = description,
                SquareMeters = squareMeters != "" ? Int32.Parse(squareMeters) : -1,
                Price = price != "" ? Int32.Parse(price) : -1,
                SharedDept = sharedDept != "" ? Int32.Parse(sharedDept) : -1,
                SharedExpenses = sharedExpenses != "" ? Int32.Parse(sharedExpenses) : -1,
                Added = DateTime.Now,
            };
        }
    }
}