using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.StartupExtensions
{
    [ExcludeFromCodeCoverage]
    public static class AuthorizationStartupExtensions
    {
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyNames.HasProviderAccount, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ProviderClaims.ProviderUkprn);
                    policy.RequireClaim(ProviderClaims.Service);
                    policy.Requirements.Add(new ClaimUkprnMatchesRouteUkprnRequirement());
                    policy.Requirements.Add(new ClaimUkprnAllowedAccessRequirement());
                });

                options.AddPolicy(PolicyNames.HasContributorOrAbovePermission, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ProviderClaims.ProviderUkprn);
                    policy.RequireClaim(ProviderClaims.Service);
                    policy.Requirements.Add(new MinimumServiceClaimRequirement(ServiceClaim.DAC));
                });
            });

            return services;
        }
    }
}