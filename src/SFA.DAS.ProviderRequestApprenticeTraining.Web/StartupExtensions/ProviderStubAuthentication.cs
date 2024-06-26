using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.StartupExtensions
{
    [ExcludeFromCodeCoverage]
    public static class ProviderStubAuthentication
    {
        public static void AddProviderStubAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("Provider-stub").AddScheme<AuthenticationSchemeOptions, ProviderStubAuthHandler>(
                "Provider-stub",
                options => { });
        }
    }
}