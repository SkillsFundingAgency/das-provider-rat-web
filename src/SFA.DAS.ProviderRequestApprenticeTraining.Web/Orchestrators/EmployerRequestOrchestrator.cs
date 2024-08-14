using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators
{
    public class EmployerRequestOrchestrator : IEmployerRequestOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ProviderSharedUIConfiguration _providerSharedUIConfiguration;

        public EmployerRequestOrchestrator(IMediator mediator, IOptions<ProviderSharedUIConfiguration> sharedUIConfiguration)
        {
            _mediator = mediator;
            _providerSharedUIConfiguration = sharedUIConfiguration.Value;
        }

        public async Task<ActiveEmployerRequestsViewModel> GetActiveEmployerRequestsViewModel(long ukprn)
        {
            var result = await _mediator.Send(new GetAggregatedEmployerRequestsQuery(ukprn));

            var model = new ActiveEmployerRequestsViewModel()
            {
                AggregatedEmployerRequests = result.AggregatedEmployerRequests.Select(request => (ActiveEmployerRequestViewModel)request).ToList(),
                BackLink = _providerSharedUIConfiguration.DashboardUrl + "account"
            };

            return model;
        }
    }
}
