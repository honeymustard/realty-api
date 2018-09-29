using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            var secrets = Configuration.GetSection("Secrets").Get<Secrets>();
            var tokens = Configuration.GetSection("Tokens").Get<Tokens>();
            var userDB = new Database(Configuration.GetSection("UserDB").Get<Credentials>());
            var realtyDB = new Database(Configuration.GetSection("RealtyDB").Get<Credentials>());

            services.AddSingleton<Secrets>(secrets);
            services.AddSingleton<Tokens>(tokens);
            services.AddSingleton<IRepository<UserDocument>>(new UserRepository(userDB));
            services.AddSingleton<IRepository<RealtyDocument>>(new RealtyRepository(realtyDB));
            services.AddSingleton<IEnvironment, Environment>();
            services.AddSingleton<IUtilities, Utilitis>();
            services.AddSingleton<IBrowser, Browser>();

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidIssuer = tokens.Issuer,
                    IssuerSigningKey = tokens.SigningKey,
                };
            });

            services.AddMemoryCache();
            services.AddMvc();
        }

        // This method gets called by the runtime.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<UserModel, UserDocument>();
                config.CreateMap<RealtyModel, RealtyDocument>();
            });

            app.UseCors(policy => policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseStatusCodePages("text/plain", "status: {0}");
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}