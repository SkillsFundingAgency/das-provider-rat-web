using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Configuration;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Extensions;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
    [SetNavigationSection(NavigationSection.Home)]
    public class EmployerRequestController : Controller
    {
        private readonly IEmployerRequestOrchestrator _orchestrator;
        private readonly ProviderUrlConfiguration _providerUrlConfiguration;
        private readonly IHttpContextAccessor _contextAccessor;

        #region Routes
        public const string ActiveRouteGet = nameof(ActiveRouteGet);
        public const string SelectRequestsToContactRouteGet = nameof(SelectRequestsToContactRouteGet);
        public const string SelectRequestsToContactRoutePost = nameof(SelectRequestsToContactRoutePost);
        public const string CancelSelectRequestsRouteGet = nameof(CancelSelectRequestsRouteGet);
        public const string SelectProviderEmailRouteGet = nameof(SelectProviderEmailRouteGet);
        public const string SelectProviderEmailRoutePost = nameof(SelectProviderEmailRoutePost);
        public const string ManageStandardsRouteGet = nameof(ManageStandardsRouteGet);
        public const string SelectProviderPhoneRouteGet = nameof(SelectProviderPhoneRouteGet);
        public const string SelectProviderPhoneRoutePost = nameof(SelectProviderPhoneRoutePost);
        public const string CheckYourAnswersRouteGet = nameof(CheckYourAnswersRouteGet);
        public const string CheckYourAnswersRoutePost = nameof(CheckYourAnswersRoutePost);
        #endregion Routes

        public EmployerRequestController(
            IEmployerRequestOrchestrator orchestrator, 
            IOptions<ProviderUrlConfiguration> options, 
            IHttpContextAccessor contextAccessor)
        {
            _orchestrator = orchestrator;
            _providerUrlConfiguration = options.Value; 
            _contextAccessor = contextAccessor;
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

        [HttpGet("email-addresses", Name = SelectProviderEmailRouteGet)]
        public async Task<IActionResult> SelectProviderEmail(EmployerRequestsParameters parameters)
        {
            var currentUserEmail = _contextAccessor.HttpContext.User.GetEmailAddress();
            var viewModel = await _orchestrator.GetProviderEmailsViewModel(
                new GetProviderEmailsParameters 
                {
                    Ukprn = parameters.Ukprn,
                    StandardReference = parameters.StandardReference,
                    UserEmailAddress = currentUserEmail
                },
                ModelState);

            if (viewModel.HasSingleEmail)
            {
                return RedirectToRoute(nameof(SelectProviderPhoneRouteGet), new { parameters.Ukprn, parameters.StandardReference });
            }

            return View(viewModel);
        }

        [HttpPost("email-addresses", Name = SelectProviderEmailRoutePost)]
        public async Task<IActionResult> SelectProviderEmail(SelectProviderEmailViewModel viewModel)
        {
            if (!await _orchestrator.ValidateProviderEmailsViewModel(viewModel, ModelState))
            {
                return RedirectToRoute(SelectProviderEmailRouteGet, new { viewModel.Ukprn, viewModel.StandardReference });
            }

            _orchestrator.UpdateProviderEmail(viewModel);

            return RedirectToRoute(nameof(SelectProviderPhoneRouteGet), new { viewModel.Ukprn, viewModel.StandardReference });
        }

        [HttpGet("manage-standards", Name = ManageStandardsRouteGet)]
        public IActionResult RedirectToManageStandards(long ukprn)
        {
            _orchestrator.ClearProviderResponse();
            return RedirectPermanent($"{_providerUrlConfiguration.CourseManagementBaseUrl}/{ukprn}/review-your-details");
        }

        [HttpGet("phone", Name = SelectProviderPhoneRouteGet)]
        public async Task<IActionResult> SelectProviderPhoneNumber(EmployerRequestsParameters parameters)
        {
            var viewModel = await _orchestrator.GetProviderPhoneNumbersViewModel(parameters, ModelState);
            if (viewModel.HasSinglePhoneNumber)
            {
                return RedirectToRoute(nameof(CheckYourAnswersRouteGet), new { parameters.Ukprn, parameters.StandardReference });
            }
            return View(viewModel);
        }

        [HttpPost("phone", Name = SelectProviderPhoneRoutePost)]
        public async Task<IActionResult> SelectProviderPhoneNumber(SelectProviderPhoneViewModel viewModel)
        {
            if (!await _orchestrator.ValidateProviderPhoneViewModel(viewModel, ModelState))
            {
                return RedirectToRoute(SelectProviderPhoneRouteGet, new { viewModel.Ukprn, viewModel.StandardReference });
            }

            _orchestrator.UpdateProviderPhone(viewModel);

            return RedirectToRoute(nameof(CheckYourAnswersRouteGet), new { viewModel.Ukprn, viewModel.StandardReference });
        }

        [HttpGet]
        [Route("cancel", Name = CancelSelectRequestsRouteGet)]
        public IActionResult Cancel(long ukprn)
        {
            _orchestrator.ClearProviderResponse();
            return RedirectToRoute(nameof(ActiveRouteGet), new { ukprn });
        }

        [HttpGet]
        [Route("check-your-answers", Name = CheckYourAnswersRouteGet)]
        public async Task<IActionResult> CheckYourAnswers(EmployerRequestsParameters parameters)
        {
            var viewModel = await _orchestrator.GetCheckYourAnswersRespondToRequestsViewModel(parameters, ModelState);

            if (!await _orchestrator.ValidateCheckYourAnswersViewModel(viewModel, ModelState))
            {
                _orchestrator.ClearProviderResponse();
                return RedirectToRoute(ActiveRouteGet, new { viewModel.Ukprn });
            }

            return View(viewModel);
        }

        [HttpPost("check-your-answers", Name = CheckYourAnswersRoutePost)]
        public IActionResult CheckYourAnswers(CheckYourAnswersRespondToRequestsViewModel viewModel)
        {
            return RedirectToRoute(nameof(CheckYourAnswersRouteGet), new { viewModel.Ukprn, viewModel.StandardReference });
        }

    }
}