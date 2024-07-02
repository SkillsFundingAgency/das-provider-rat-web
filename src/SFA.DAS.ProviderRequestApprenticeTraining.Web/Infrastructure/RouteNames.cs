using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class RouteNames
    {
        public const string HomeGetStart = nameof(HomeGetStart);
        
        // exact route name is required by the SFA.DAS.Provider.Shared.UI menu link
        public const string ProviderSignOut = "provider-signout";

    }
}