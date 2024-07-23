using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Extensions;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
    [SetNavigationSection(NavigationSection.Home)]
    public class EmployerRequestController : Controller
    {
        private readonly IEmployerRequestOrchestrator _orchestrator;
        private readonly IHttpContextAccessor _contextAccessor;

        #region Routes
        public const string ActiveRouteGet = nameof(ActiveRouteGet);
        #endregion Routes

        public EmployerRequestController(IEmployerRequestOrchestrator orchestrator, IHttpContextAccessor contextAccessor)
        {
            _orchestrator = orchestrator;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("active", Name = ActiveRouteGet)]
        public async Task<IActionResult> Active()
        {
            var ukprn = _contextAccessor.HttpContext.User.GetUkprn();
            
            return View(await _orchestrator.GetActiveEmployerRequestsViewModel(long.Parse(ukprn)));
        }
    }
}