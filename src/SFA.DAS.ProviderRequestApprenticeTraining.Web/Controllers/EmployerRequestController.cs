using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Configuration;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
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
        private readonly ProviderUrlConfiguration _providerUrlConfiguration;

        #region Routes
        public const string ActiveRouteGet = nameof(ActiveRouteGet);
        public const string SelectRequestsToContactRouteGet = nameof(SelectRequestsToContactRouteGet);
        public const string SelectRequestsToContactRoutePost = nameof(SelectRequestsToContactRoutePost);
        public const string CancelSelectRequestsRouteGet = nameof(CancelSelectRequestsRouteGet);
        public const string SelectProviderEmailRouteGet = nameof(SelectProviderEmailRouteGet);
        public const string SelectProviderEmailRoutePost = nameof(SelectProviderEmailRoutePost);
        public const string ManageStandardsRouteGet = nameof(ManageStandardsRouteGet);
        #endregion Routes

        public EmployerRequestController(IEmployerRequestOrchestrator orchestrator, IOptions<ProviderUrlConfiguration> options)
        {
            _orchestrator = orchestrator;
            _providerUrlConfiguration = options.Value;    
        }

        [HttpGet("{ukprn}/active", Name = ActiveRouteGet)]
        public async Task<IActionResult> Active(long ukprn)
        {
            _orchestrator.StartProviderResponse(ukprn);
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
                return RedirectToRoute(SelectRequestsToContactRouteGet, new { viewModel.Ukprn, viewModel.StandardReference });
            }

            await _orchestrator.UpdateSelectedRequests(viewModel);

            return RedirectToRoute(nameof(SelectProviderEmailRouteGet), new { viewModel.Ukprn, viewModel.StandardReference });
        }

        [HttpGet("email", Name = SelectProviderEmailRouteGet)]
        public async Task<IActionResult> SelectProviderEmail(EmployerRequestsParameters parameters)
        {
            return View(await _orchestrator.GetProviderEmailsViewModel(parameters, ModelState));
        }

        [HttpPost("email", Name = SelectProviderEmailRoutePost)]
        public async Task<IActionResult> SelectProviderEmail(SelectProviderEmailViewModel viewModel)
        {
            if (!await _orchestrator.ValidateProviderEmailsViewModel(viewModel, ModelState))
            {
                return RedirectToRoute(SelectProviderEmailRouteGet, new { viewModel.Ukprn, viewModel.StandardReference });
            }

            _orchestrator.UpdateProviderEmail(viewModel);

            return RedirectToRoute(nameof(SelectProviderEmailRouteGet), new { viewModel.Ukprn, viewModel.StandardReference });
        }

        [HttpGet("managestandards", Name = ManageStandardsRouteGet)]
        public IActionResult RedirectToManageStandards(long ukprn)
        {
            _orchestrator.EndSession();
            return RedirectPermanent($"{_providerUrlConfiguration.CourseManagementBaseUrl}/{ukprn}/review-your-details");
        }


        [HttpGet]
        [Route("cancel", Name = CancelSelectRequestsRouteGet)]
        public IActionResult Cancel(long ukprn)
        {
            return RedirectToRoute(nameof(ActiveRouteGet), new { ukprn });
        }

    }
}