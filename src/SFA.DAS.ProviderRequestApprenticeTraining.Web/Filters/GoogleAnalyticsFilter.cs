using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using System;
using System.Linq;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Filters
{
    public class GoogleAnalyticsFilter : ActionFilterAttribute
    {
        private readonly ILogger<GoogleAnalyticsFilter> _logger;

        public GoogleAnalyticsFilter(ILogger<GoogleAnalyticsFilter> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var userId = filterContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ProviderClaims.DisplayName))?.Value;
                var ukprn = filterContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ProviderClaims.ProviderUkprn))?.Value;

                var controller = filterContext.Controller as Controller;
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

            base.OnActionExecuting(filterContext);
        }
    }
}
