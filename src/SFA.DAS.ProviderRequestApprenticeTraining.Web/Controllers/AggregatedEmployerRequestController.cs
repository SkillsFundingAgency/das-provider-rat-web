using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Infrastructure;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using System.Threading.Tasks;
using System.Linq;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.HasProviderAccount))]
    [SetNavigationSection(NavigationSection.EmployerDemand)]
    public class AggregatedEmployerRequestController : Controller
    {
        private readonly IMediator _mediator;
        public AggregatedEmployerRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("aggregated-employer-requests", Name = RouteNames.AggregatedEmployerRequests)]
        public async Task<IActionResult> AggregatedEmployerRequests()
        {
            var result = await _mediator.Send(new GetAggregatedEmployerRequestsQuery());

            var model = new AggregatedEmployerRequestsViewModel()
            {
                AggregatedEmployerRequests = result.AggregatedEmployerRequests.Select(request => (AggregatedEmployerRequestViewModel)request).ToList()
            };

            return View(model);
        }
    }
}