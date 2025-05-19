using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
    [SetNavigationSection(NavigationSection.Home)]
    public class HomeController : Controller
    {
        private readonly ProviderSharedUIConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<HomeController> _logger;

        #region Routes
        public const string StartRouteGet = nameof(StartRouteGet);
        #endregion Routes

        public HomeController(IOptions<ProviderSharedUIConfiguration> configuration, IHttpContextAccessor contextAccessor, ILogger<HomeController> logger)
        {
            _configuration = configuration?.Value;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("", Order = 0)]
        public IActionResult Index()
        {
            _logger.LogInformation("Index called");
            // the default action is to return to the provider portal, used for provider sign-out
            return Redirect(_configuration.DashboardUrl + "account");
        }

#if DEBUG
        [ExcludeFromCodeCoverage]
        [HttpGet("start", Name = StartRouteGet)]
        public IActionResult Start()
        {
            var ukprn = _contextAccessor.HttpContext.User.GetUkprn();
            return RedirectToRoute(EmployerRequestController.ActiveRouteGet, new { ukprn });
        }
#endif
    }
}