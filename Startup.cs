using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;

namespace Honeymustard
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json",
                    optional: false,
                    reloadOnChange: true)
                .AddJsonFile("secrets.json",
                    optional: false,
                    reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        // This method gets called by the runtime.
        public void ConfigureServices(IServiceCollection services)
        {
            var credentials = Configuration.GetSection("MongoDB").Get<Credentials>();

            services.AddSingleton<ICredentials>(credentials);
            services.AddSingleton<IDatabase, Database>();
            services.AddSingleton<RealtyRepository, RealtyRepository>();
            services.AddSingleton<IPathService, PathService>();
            services.AddSingleton<IUtilityService, UtilityService>();
            services.AddSingleton<IHTTPService, HTTPService>();
            services.AddMvc();
        }

        // This method gets called by the runtime.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Mapper.Initialize(config => config.CreateMap<RealtyModel, RealtyDocument>());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}