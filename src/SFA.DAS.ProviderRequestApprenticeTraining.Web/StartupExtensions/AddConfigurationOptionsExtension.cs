using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.StartupExtensions
{
    [ExcludeFromCodeCoverage]
    public static class AddConfigurationOptionsExtension
    {
        public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ProviderRequestApprenticeTrainingOuterApiConfig>(configuration.GetSection(nameof(ProviderRequestApprenticeTrainingOuterApiConfig)));;

            services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRequestApprenticeTrainingOuterApiConfig>>().Value);

            return services;
        }
    }
}