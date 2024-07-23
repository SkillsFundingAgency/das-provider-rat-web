using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequests;
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
    }
}
