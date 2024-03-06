using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Http.Configuration;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ProviderRequestApprenticeTrainingOuterApiConfiguration : IApimClientConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string SubscriptionKey { get; set; }
        public string ApiVersion { get; set; }
    }
}