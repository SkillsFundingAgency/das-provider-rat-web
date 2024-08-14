using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using RestEase.HttpClientFactory;
using SFA.DAS.Http.Configuration;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Configuration;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.StartupExtensions
{
    [ExcludeFromCodeCoverage]
    public static class AddServiceRegistrationExtension
    {
        public static IServiceCollection AddServiceRegistrations(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAggregatedEmployerRequestsQuery).Assembly));

            services.AddHttpContextAccessor();
            services.AddTransient<ITrainingProviderService, TrainingProviderService>();
            services.AddSingleton<IAuthorizationHandler, ClaimUkprnMatchesRouteUkprnAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ClaimUkprnAllowedAccessAuthorizationHandler>();

            return services;
        }

        public static IServiceCollection AddOuterApi(this IServiceCollection services, ProviderRequestApprenticeTrainingOuterApiConfiguration configuration)
        {
            services.AddHealthChecks();
            services.AddScoped<Http.MessageHandlers.DefaultHeadersHandler>();
            services.AddScoped<Http.MessageHandlers.LoggingMessageHandler>();
            services.AddScoped<Http.MessageHandlers.ApimHeadersHandler>();

            services
                .AddRestEaseClient<IProviderRequestApprenticeTrainingOuterApi>(configuration.ApiBaseUrl)
                .AddHttpMessageHandler<Http.MessageHandlers.DefaultHeadersHandler>()
                .AddHttpMessageHandler<Http.MessageHandlers.ApimHeadersHandler>()
                .AddHttpMessageHandler<Http.MessageHandlers.LoggingMessageHandler>();

            services.AddTransient<IEmployerRequestOrchestrator, EmployerRequestOrchestrator>();
            services.AddTransient<IApimClientConfiguration>((_) => configuration);

            return services;
        }
    }
}