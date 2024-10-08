using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using RestEase.HttpClientFactory;
using SFA.DAS.Http.Configuration;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Configuration;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services.SessionStorage;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.StartupExtensions
{
    [ExcludeFromCodeCoverage]
    public static class AddServiceRegistrationExtension
    {
        public static IServiceCollection AddServiceRegistrations(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAggregatedEmployerRequestsQuery).Assembly));

            services.AddTransient<ITrainingProviderService, TrainingProviderService>();
            services.AddSingleton<IAuthorizationHandler, ClaimUkprnMatchesRouteUkprnAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ClaimUkprnAllowedAccessAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, MinimumServiceClaimAuthorizationHandler>();
            services.AddTransient<ISessionStorageService, SessionStorageService>();

            services.AddTransient(sp => new EmployerRequestOrchestratorValidators
            {
                SelectedRequestsModelValidator = sp.GetRequiredService<IValidator<EmployerRequestsToContactViewModel>>(),
                SelectProviderEmailViewModelValidator = sp.GetRequiredService<IValidator<SelectProviderEmailViewModel>>(),
                SelectProviderPhoneViewModelValidator = sp.GetRequiredService<IValidator<SelectProviderPhoneViewModel>>(),
                CheckYourAnswersViewModelValidator = sp.GetRequiredService<IValidator<CheckYourAnswersRespondToRequestsViewModel>>(),
            });
            services.AddTransient<IEmployerRequestOrchestrator, EmployerRequestOrchestrator>();

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

            services.AddTransient<IApimClientConfiguration>((_) => configuration);

            return services;
        }
    }
}