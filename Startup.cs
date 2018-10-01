using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;
using MongoDB.Driver;

namespace Honeymustard
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        public Tokens Tokens { get; set; }
        public ICredentials Credentials { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
            Tokens = Configuration.GetSection("Tokens").Get<Tokens>();
            Credentials = Configuration.GetSection("Credentials").Get<Credentials>();

            Mapper.Initialize(config =>
            {
                config.CreateMap<UserModel, UserDocument>();
                config.CreateMap<RealtyModel, RealtyDocument>();
            });
        }

        /// <summary>
        /// Adds services to the service container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Tokens>(Tokens);
            services.AddSingleton<ICredentials>(Credentials);
            services.AddSingleton<IDatabase, Database>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IRealtyRepository, RealtyRepository>();
            services.AddSingleton<IEnvironment, Environment>();
            services.AddSingleton<IUtilities, Utilitis>();
            services.AddSingleton<IBrowser, Browser>();

            // Disables authorization for development
            services.AddAuthorization(options =>
            {
                if (Environment.IsDevelopment())
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAssertion(_ => true)
                        .Build();
                }
            });

            // Enables authentication with Json Web tokens
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = Tokens.GetValidationParameters();
            });

            services.AddMemoryCache();
            services.AddMvc();
        }

        /// <summary>
        /// Configures the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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