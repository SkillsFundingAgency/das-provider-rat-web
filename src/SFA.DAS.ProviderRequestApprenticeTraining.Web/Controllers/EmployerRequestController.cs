using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Extensions;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
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
        public const string SelectRequestsToContactRouteGet = nameof(SelectRequestsToContactRouteGet);
        public const string SelectRequestsToContactRoutePost = nameof(SelectRequestsToContactRoutePost);
        public const string CancelSelectRequestsRouteGet = nameof(CancelSelectRequestsRouteGet);
        #endregion Routes

        public EmployerRequestController(IEmployerRequestOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [HttpGet("{ukprn}/active", Name = ActiveRouteGet)]
        public async Task<IActionResult> Active(long ukprn)
        {          
            return View(await _orchestrator.GetActiveEmployerRequestsViewModel(ukprn));
        }

        [HttpGet("select", Name = SelectRequestsToContactRouteGet)]
        public async Task<IActionResult> SelectRequestsToContact(EmployerRequestsParameters parameters)
        {
            return View(await _orchestrator.GetEmployerRequestsByStandardViewModel(parameters, ModelState));
        }

        [HttpPost("select", Name = SelectRequestsToContactRoutePost)]
        public async Task<IActionResult> SelectRequestsToContact(EmployerRequestsToContactViewModel viewModel)
        {
            if (!await _orchestrator.ValidateEmployerRequestsToContactViewModel(viewModel, ModelState))
            {
                return RedirectToRoute(SelectRequestsToContactRouteGet, new { viewModel.Ukprn, viewModel.StandardReference, viewModel.MySelectedRequests });
            }

            _orchestrator.UpdateSelectedRequests(viewModel);

            return RedirectToRoute(nameof(SelectRequestsToContactRouteGet), new { viewModel.Ukprn, viewModel.StandardReference });
        }

        [HttpGet]
        [Route("cancel", Name = CancelSelectRequestsRouteGet)]
        public IActionResult Cancel()
        {
            return RedirectToRoute(nameof(ActiveRouteGet));
        }

    }
}