using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Web.Orchestrators
{
    public class AggregatedEmployerRequestOrchestrator : IAggregatedEmployerRequestOrchestrator
    {
        private readonly IMediator _mediator;

        public AggregatedEmployerRequestOrchestrator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<GetViewAggregatedEmployerRequestsViewModel> GetViewAggregatedEmployerRequestsViewModel()
        {
            var result = await _mediator.Send(new GetAggregatedEmployerRequestsQuery());

            return new GetViewAggregatedEmployerRequestsViewModel()
            {
                AggregatedEmployerRequests = result
            };
        }
    }
}