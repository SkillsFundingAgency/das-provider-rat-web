using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Extensions;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.SelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
    [SetNavigationSection(NavigationSection.Home)]
    public class EmployerRequestController : Controller
    {
        private readonly IEmployerRequestOrchestrator _orchestrator;

        #region Routes
        public const string ActiveRouteGet = nameof(ActiveRouteGet);
        public const string SelectRequestsRouteGet = nameof(SelectRequestsRouteGet);
        public const string SelectRequestsRoutePost = nameof(SelectRequestsRoutePost);
        public const string CancelSelectRequestsRouteGet = nameof(CancelSelectRequestsRouteGet);
        #endregion Routes

        public EmployerRequestController(IEmployerRequestOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [HttpGet("active", Name = ActiveRouteGet)]
        public async Task<IActionResult> Active()
        {
            var ukprn = HttpContext.User.GetUkprn();
            
            return View(await _orchestrator.GetActiveEmployerRequestsViewModel(long.Parse(ukprn)));
        }

        [HttpGet("select/{standardReference}", Name = SelectRequestsRouteGet)]
        public async Task<IActionResult> SelectEmployerRequests(string standardReference)
        {
            var ukprn = HttpContext.User.GetUkprn();

            return View(await _orchestrator.GetSelectEmployerRequestsViewModel(long.Parse(ukprn), standardReference));
        }

        [HttpPost("select/{standardReference}", Name = SelectRequestsRoutePost)]
        public IActionResult SelectEmployerRequests(SelectedRequestsViewModel viewModel)
        {
            _orchestrator.UpdateSelectedRequests(viewModel);
            
            return RedirectToRoute(nameof(SelectRequestsRouteGet));
        }

        [HttpGet]
        [Route("cancel", Name = CancelSelectRequestsRouteGet)]
        public IActionResult Cancel()
        {
            return RedirectToRoute(nameof(ActiveRouteGet));
        }

    }
}