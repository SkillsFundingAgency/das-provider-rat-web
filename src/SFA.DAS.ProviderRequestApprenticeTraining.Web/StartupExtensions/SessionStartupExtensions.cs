using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.StartupExtensions
{
    [ExcludeFromCodeCoverage]
    public static class SessionStartupExtensions
    {
        public static IServiceCollection AddSession(this IServiceCollection services, ProviderRequestApprenticeTrainingWebConfiguration configWeb)
        {
            services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromMinutes(20);
                opt.Cookie = new CookieBuilder()
                {
                    Name = ".ProviderRequestApprenticeTraining.Session",
                    HttpOnly = true
                };
            });

            return services;
        }
    }
}
