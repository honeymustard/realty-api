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

namespace Honeymustard
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

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
                options.TokenValidationParameters = tokens.GetValidationParameters();
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