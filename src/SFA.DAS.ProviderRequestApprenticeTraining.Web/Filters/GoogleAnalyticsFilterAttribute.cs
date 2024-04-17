using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Filters
{
    [ExcludeFromCodeCoverage]
    public class GoogleAnalyticsFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger<GoogleAnalyticsFilterAttribute> _logger;

        public GoogleAnalyticsFilterAttribute(ILogger<GoogleAnalyticsFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ProviderClaims.DisplayName))?.Value;
                var ukprn = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ProviderClaims.ProviderUkprn))?.Value;

                var controller = context.Controller as Controller;
                if (controller != null)
                {
                    controller.ViewBag.GaData = new GaData
                    {
                        UserId = userId ?? string.Empty,
                        UkPrn = ukprn ?? string.Empty
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GoogleAnalyticsFilter Cannot set GaData for Provider");
            }

            base.OnActionExecuting(context);
        }
    }
}
