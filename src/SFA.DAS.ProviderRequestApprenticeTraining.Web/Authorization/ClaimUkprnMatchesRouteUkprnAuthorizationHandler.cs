using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization
{
    [ExcludeFromCodeCoverage]
    public class ClaimUkprnMatchesRouteUkprnAuthorizationHandler : AuthorizationHandler<ClaimUkprnMatchesRouteUkprnRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimUkprnMatchesRouteUkprnAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimUkprnMatchesRouteUkprnRequirement requirement)
        {
            if (!DoesRouteUkprnMatchClaim(context))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        private bool DoesRouteUkprnMatchClaim(AuthorizationHandlerContext context)
        {
            if (!context.User.HasClaim(c => c.Type.Equals(ProviderClaims.ProviderUkprn)))
            {
                return false;
            }

            if (_httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey("ukprn"))
            {
                var ukprnFromUrl = _httpContextAccessor.HttpContext.Request.RouteValues["ukprn"].ToString();
                var ukprn = context.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn)).Value;

                return ukprn.Equals(ukprnFromUrl);
            }

            return true;
        }
    }
}