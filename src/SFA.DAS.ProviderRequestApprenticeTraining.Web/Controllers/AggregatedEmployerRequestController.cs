using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerRequestApprenticeTraining.Web.Orchestrators;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Infrastructure;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
    [SetNavigationSection(NavigationSection.EmployerDemand)]
    public class AggregatedEmployerRequestController : Controller
    {
        private readonly IAggregatedEmployerRequestOrchestrator _orchestrator;
        public AggregatedEmployerRequestController(IAggregatedEmployerRequestOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [HttpGet("aggregated-employer-requests", Name = RouteNames.AggregatedEmployerRequests)]
        public async Task<IActionResult> AggregatedEmployerRequests()
        {
            var viewModel = await _orchestrator.GetViewAggregatedEmployerRequestsViewModel();
            return View(viewModel);
        }
    }
}