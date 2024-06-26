using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Infrastructure;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
    [SetNavigationSection(NavigationSection.EmployerDemand)]
    public class HomeController : Controller
    {
        private readonly ProviderSharedUIConfiguration _configuration;

        public HomeController(IOptions<ProviderSharedUIConfiguration> configuration)
        {
            _configuration = configuration?.Value;
        }

        [AllowAnonymous]
        [HttpGet("", Order = 0)]
        public IActionResult Index()
        {
            // the default action is to return to the provider portal, used for provider sign-out
            return Redirect(_configuration.DashboardUrl);
        }

        [HttpGet("start", Name = RouteNames.HomeGetStart)]
        public IActionResult Start()
        {
            return View();
        }
    }
}