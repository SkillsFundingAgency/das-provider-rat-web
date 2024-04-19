using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Provider.Shared.UI.Models;
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
            services.Configure<ProviderRequestApprenticeTrainingOuterApiConfiguration>(configuration.GetSection(nameof(ProviderRequestApprenticeTrainingOuterApiConfiguration)));;
            services.Configure<ProviderSharedUIConfiguration>(configuration.GetSection(nameof(ProviderSharedUIConfiguration)));

            services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRequestApprenticeTrainingOuterApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderSharedUIConfiguration>>().Value);

            return services;
        }
    }
}