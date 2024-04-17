using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.Provider.Shared.UI.Startup;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Configuration;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Filters;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.StartupExtensions;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration.BuildDasConfiguration();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddLogging(builder =>
            {
                builder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
                builder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Information);
            });

            services.AddConfigurationOptions(_configuration);

            var configurationOuterApi = _configuration.GetSection<ProviderRequestApprenticeTrainingOuterApiConfiguration>();

            services
                .AddSingleton(configurationOuterApi);

            services.AddControllersWithViews();

            services
                .Configure<RouteOptions>(options =>
                {
                    options.LowercaseUrls = true;
                })
                .AddMvc(options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    options.Filters.Add(new TypeFilterAttribute(typeof(GoogleAnalyticsFilterAttribute)));

                })
                .AddControllersAsServices()
                .SetDefaultNavigationSection(NavigationSection.Home)
                .EnableGoogleAnalytics()
                .SetDfESignInConfiguration(true)
                .SetZenDeskConfiguration(_configuration.GetSection("ProviderZenDeskSettings").Get<ZenDeskConfiguration>());

            services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<Startup>();

            services
                .AddProviderAuthentication(_configuration)
                .AddAuthorizationPolicies()
                .AddCookieTempDataProvider()
                .AddDasHealthChecks()
                .AddServiceRegistrations()
                .AddOuterApi(configurationOuterApi)
                .AddApplicationInsightsTelemetry()
                .AddProviderUiServiceRegistration(_configuration);

#if DEBUG
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
#endif
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCookiePolicy();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseDasHealthChecks();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<SecurityHeadersMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}