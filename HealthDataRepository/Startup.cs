using Hangfire;
using Hangfire.MySql;
using HealthDataRepository.Models;
using HealthDataRepository.Repositories;
using HealthDataRepository.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

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
            var appConfiguration = Configuration.GetSection("HealthData");
            var connectionString = Configuration.GetConnectionString("HealthDataRepositoryContext");

            services.AddHttpClient("apiclient", client =>
            {
                var host = new Uri(appConfiguration.GetValue<string>("GatekeeperUrl")).Host;
                client.BaseAddress = new Uri($"https://{host}");
            });
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IActivityTypeRepository, ActivityTypeRepository>();
            services.AddScoped<IEmailRecordRepository, EmailRecordRepository>();
            services.AddSingleton<IApiClient, ApiClient>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<HealthDataRepositoryContext>(options => options.UseMySql(connectionString));

            services.AddHangfire(x => x.UseStorage(new MySqlStorage(connectionString, new MySqlStorageOptions())));

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

            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseHangfireDashboard();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseHangfireServer();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=ActivityTypes}/{action=Index}/{id?}");
            });

            RecurringJob.AddOrUpdate<EmailManager>(es => es.SendScheduledEmails(), Cron.Hourly);
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
