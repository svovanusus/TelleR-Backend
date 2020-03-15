using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TelleR.Configuration;
using Microsoft.EntityFrameworkCore;
using TelleR.Data.Contexts;
using System;
using TelleR.Unity;
using Autofac.Extensions.DependencyInjection;
using Unity;
using TelleR.Logic.Services;
using TelleR.Logic.Services.Impl;
using TelleR.Logic.Tools;
using TelleR.Logic.Tools.Impl;

namespace TelleRPlatformApi
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(o => o.UseSqlServer(Configuration.GetConnectionString("DebugConnectionStringMainDatabase"), x => x.MigrationsAssembly("TelleR.Migrations")));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthConfig.ISSUER,

                        ValidateAudience = true,
                        ValidAudience = AuthConfig.AUDIENCE,

                        ValidateLifetime = false,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = AuthConfig.GetSymmetricSecurityKey()
                    };
                });

            services.AddSingleton<IUnityContainer>(UnityContainerFactory.Container);
            services.AddScoped<ITellerDatabaseUnitOfWorkFactory, TellerDatabaseUnitOfWorkFactoryImpl>();

            services.AddScoped<IAwsService, AwsServiceImpl>();
            services.AddScoped<IBlogService, BlogServiceImpl>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
