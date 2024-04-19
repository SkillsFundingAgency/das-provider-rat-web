using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization
{
    public class ClaimUkprnAllowedAccessAuthorizationHandler : AuthorizationHandler<ClaimUkprnAllowedAccessRequirement>
    {
        private readonly ITrainingProviderService _trainingProviderService;
        private readonly IConfiguration _configuration;
        private readonly ProviderSharedUIConfiguration _providerSharedUiConfiguration;

        public ClaimUkprnAllowedAccessAuthorizationHandler(
            ITrainingProviderService trainingProviderService,
            IConfiguration configuration,
            ProviderSharedUIConfiguration providerSharedUiConfiguration)
        {
            _trainingProviderService = trainingProviderService;
            _providerSharedUiConfiguration = providerSharedUiConfiguration;
            _configuration = configuration;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimUkprnAllowedAccessRequirement requirement)
        {
            HttpContext currentContext;
            switch (context.Resource)
            {
                case HttpContext resource:
                    currentContext = resource;
                    break;
                case AuthorizationFilterContext authorizationFilterContext:
                    currentContext = authorizationFilterContext.HttpContext;
                    break;
                default:
                    currentContext = null;
                    break;
            }

            var ukprnClaim = context.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn))?.Value;
            if (ukprnClaim == null || !int.TryParse(ukprnClaim, out int ukprn))
            {
                context.Fail();
                return;
            }

            // redirect the user to PAS 403 forbidden page if the provider is not allowed access, bypass the check
            // if the stub is enabled and allow any provider to access the service, this is used for local development
            if (!GetUseStubProviderValidationSetting() && !await _trainingProviderService.CanProviderAccessService(ukprn))
            {
                currentContext?.Response.Redirect($"{_providerSharedUiConfiguration.DashboardUrl}/error/403/invalid-status");
            }

            context.Succeed(requirement);
        }

        private bool GetUseStubProviderValidationSetting()
        {
            var value = _configuration.GetSection("UseStubProviderValidation").Value;
            return value != null && bool.TryParse(value, out var useStubProviderValidation) && useStubProviderValidation;
        }
    }
}
