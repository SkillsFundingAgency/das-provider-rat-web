using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization
{
    public class MinimumServiceClaimRequirement : IAuthorizationRequirement
    {
        public ServiceClaim MinimumServiceClaim { get; set; }

        public MinimumServiceClaimRequirement(ServiceClaim minimumServiceClaim)
        {
            MinimumServiceClaim = minimumServiceClaim;
        }
    }
}
