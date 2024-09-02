using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Extensions;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization
{
    public class MinimumServiceClaimAuthorizationHandler : AuthorizationHandler<MinimumServiceClaimRequirement>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumServiceClaimRequirement requirement)
        {
            if (context.User.HasPermission(requirement.MinimumServiceClaim)) context.Succeed(requirement);
            else context.Fail();

            return Task.CompletedTask;
        }
    }
}
