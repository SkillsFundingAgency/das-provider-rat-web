using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUkprn(this ClaimsPrincipal user)
        {
            return user.FindFirst(ProviderClaims.ProviderUkprn)?.Value;
        }

        public static string GetEmailAddress(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ProviderClaims.Email);
        }

        public static string GetFirstName(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ProviderClaims.GivenName);
        }

        public static string GetDisplayName(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ProviderClaims.DisplayName);
        }

        public static string GetSub(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ProviderClaims.Sub);
        }

        public static bool HasPermission(this ClaimsPrincipal user, ServiceClaim minimumServiceClaim)
        {
            var serviceClaims = user
                .FindAll(c => c.Type == ProviderClaims.Service)
                .Select(c => c.Value)
                .ToList();

            ServiceClaim? highestClaim = null;

            if (serviceClaims.Contains(ServiceClaim.DAA.ToString())) highestClaim = ServiceClaim.DAA;
            else if (serviceClaims.Contains(ServiceClaim.DAB.ToString())) highestClaim = ServiceClaim.DAB;
            else if (serviceClaims.Contains(ServiceClaim.DAC.ToString())) highestClaim = ServiceClaim.DAC;
            else if (serviceClaims.Contains(ServiceClaim.DAV.ToString())) highestClaim = ServiceClaim.DAV;

            return highestClaim.HasValue && highestClaim.Value >= minimumServiceClaim;
        }
    }
}