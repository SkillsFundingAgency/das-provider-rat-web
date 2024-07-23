using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators
{
    public class EmployerRequestOrchestrator : IEmployerRequestOrchestrator
    {
        private readonly IMediator _mediator;

        public EmployerRequestOrchestrator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActiveEmployerRequestsViewModel> GetActiveEmployerRequestsViewModel(long ukprn)
        {
            var result = await _mediator.Send(new GetAggregatedEmployerRequestsQuery(ukprn));

            var model = new ActiveEmployerRequestsViewModel()
            {
                AggregatedEmployerRequests = result.AggregatedEmployerRequests.Select(request => (ActiveEmployerRequestViewModel)request).ToList()
            };

            return model;
        }

        public async Task<SelectEmployerRequestsViewModel> GetSelectEmployerRequestsViewModel(long ukprn, string standardReference)
        {
            var result = await _mediator.Send(new GetSelectEmployerRequestsQuery(ukprn, standardReference));

            var model = new SelectEmployerRequestsViewModel()
            {
                SelectEmployerRequests = result.SelectEmployerRequestsResponse.EmployerRequests.Select(request => (SelectEmployerRequestViewModel)request).ToList()
            };

            return model;
        }
    }
}
