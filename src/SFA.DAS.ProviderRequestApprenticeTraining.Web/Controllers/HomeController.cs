using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Infrastructure;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
    [SetNavigationSection(NavigationSection.EmployerDemand)]
    [Route("{ukprn}")]
    public class HomeController : Controller
    {
     
        public HomeController()
        {
        }

        [HttpGet("", Name = RouteNames.HomeGetIndex, Order = 0)]
        public IActionResult Index()
        {
            return View();
        }
    }
}