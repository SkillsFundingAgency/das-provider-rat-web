using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.DfESignIn.Auth.AppStart;
using SFA.DAS.DfESignIn.Auth.Enums;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.StartupExtensions
{
    [ExcludeFromCodeCoverage]
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddProviderAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration["StubProviderAuth"] != null && configuration["StubProviderAuth"].Equals("true", StringComparison.CurrentCultureIgnoreCase))
            {
                services.AddProviderStubAuthentication();
            }
            else
            {
                services.AddAndConfigureDfESignInAuthentication(
                    configuration,
                    "SFA.DAS.ProviderApprenticeshipService",
                    typeof(CustomServiceRole),
                    ClientName.ProviderRoatp,
                    "/sign-out",
                    "");
            }

            return services;
        }
    }
}