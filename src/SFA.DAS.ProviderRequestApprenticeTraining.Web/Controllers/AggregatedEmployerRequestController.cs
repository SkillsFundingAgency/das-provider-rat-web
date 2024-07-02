using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using System.Threading.Tasks;
using System.Linq;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
    [SetNavigationSection(NavigationSection.Home)]
    public class AggregatedEmployerRequestController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;

        #region RouteNames
        public const string AggregatedEmployerRequestsRouteGet = nameof(AggregatedEmployerRequestsRouteGet);
        public const string SelectEmployerRequestsRouteGet = nameof(SelectEmployerRequestsRouteGet);
        public const string SelectEmployerRequestsRoutePost = nameof(SelectEmployerRequestsRoutePost);
        public const string CancelSelectEmployerRequestRouteGet = nameof(CancelSelectEmployerRequestRouteGet);
        #endregion

        public AggregatedEmployerRequestController(IMediator mediator, IHttpContextAccessor contextAssessor)
        {
            _mediator = mediator;
            _contextAccessor = contextAssessor;
        }

        [HttpGet("aggregated-employer-requests", Name = AggregatedEmployerRequestsRouteGet)]
        public async Task<IActionResult> AggregatedEmployerRequests()
        {
            var result = await _mediator.Send(new GetAggregatedEmployerRequestsQuery());

            var model = new AggregatedEmployerRequestsViewModel()
            {
                AggregatedEmployerRequests = result.AggregatedEmployerRequests.Select(request => (AggregatedEmployerRequestViewModel)request).ToList()
            };

            return View(model);
        }

        [HttpGet("select-employer-requests/{standardReference}", Name = SelectEmployerRequestsRouteGet)]
        public async Task<IActionResult> SelectEmployerRequests(string standardReference)
        {
            var ukprn = _contextAccessor.HttpContext.User.FindFirst(c => c.Type.Equals(ProviderClaims.ProviderUkprn)).Value;

            var result = await _mediator.Send(new GetSelectEmployerRequestsQuery()
            {
                StandardReference = standardReference,
                Ukprn = int.Parse(ukprn)
            });

            var model = (SelectEmployerRequestsViewModel)result;

            return View(model);
        }

        [HttpPost("select-employer-requests", Name = SelectEmployerRequestsRoutePost)]
        public IActionResult SelectEmployerRequests(SelectEmployerRequestsViewModel model)
        {
            return RedirectToRoute(nameof(AggregatedEmployerRequestsRouteGet));
        }

        [HttpGet]
        [Route("cancel", Name = CancelSelectEmployerRequestRouteGet)]
        public IActionResult Cancel()
        {
            return RedirectToRoute(nameof(AggregatedEmployerRequestsRouteGet));
        }
    }
}