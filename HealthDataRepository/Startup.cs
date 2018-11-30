using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HealthDataRepository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using HealthDataRepository.Repositories;

namespace HealthDataRepository
{
    public class Startup
    {

        private readonly IHostingEnvironment environment;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            this.environment = environment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IActivityTypeRepository, ActivityTypeRepository>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<HealthDataRepositoryContext>(options =>
                    options.UseMySql(Configuration.GetConnectionString("HealthDataRepositoryContext")));

            if (!environment.IsDevelopment())
            {
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    var proxyAddresses = Dns.GetHostAddresses("http://nginx");
                    foreach (var ip in proxyAddresses)
                    {
                        options.KnownProxies.Add(ip);
                    }
                });
            }

            var appConfiguration = Configuration.GetSection("HealthData");
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = appConfiguration.GetValue<string>("GatekeeperUrl");
                options.ClientId = appConfiguration.GetValue<string>("ClientId");
                options.ClientSecret = appConfiguration.GetValue<string>("ClientSecret");
                options.ResponseType = "code id_token";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("profile");
                options.Scope.Add("offline_access");
                options.ClaimActions.MapJsonKey("locale", "locale");
                options.ClaimActions.MapJsonKey("user_type", "user_type");
            })
            .AddIdentityServerAuthentication("token", options =>
            {
                options.Authority = appConfiguration.GetValue<string>("GatekeeperUrl");
                options.ApiName = appConfiguration.GetValue<string>("ApiResourceName");
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", pb => pb.RequireClaim("user_type", "administrator"));

                // Coordinator policy allows both Coordinators and Administrators
                options.AddPolicy("Coordinator", pb => pb.RequireClaim("user_type", new[] { "administrator", "coordinator" }));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                UpdateDatabase(app);
                app.UsePathBase("/health-data-repository");
                app.Use((context, next) =>
                {
                    context.Request.PathBase = new PathString("/health-data-repository");
                    return next();
                });
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }


        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<HealthDataRepositoryContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
