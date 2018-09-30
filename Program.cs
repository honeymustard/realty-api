using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Honeymustard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var variables = new Dictionary<string, string> {
                { WebHostDefaults.EnvironmentKey, "Production" }
            };

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",
                    optional: false,
                    reloadOnChange: true)
                .AddInMemoryCollection(variables)
                .AddEnvironmentVariables("ASPNETCORE_")
                .AddCommandLine(args)
                .Build();

            new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseKestrel()
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}