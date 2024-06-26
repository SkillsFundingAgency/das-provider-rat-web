

using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Types;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequests
{
    public class GetAggregatedEmployerRequestsQueryHandler : IRequestHandler<GetAggregatedEmployerRequestsQuery, List<AggregatedEmployerRequest>>
    {
        private readonly IProviderRequestApprenticeTrainingOuterApi _outerApi;

        public GetAggregatedEmployerRequestsQueryHandler(IProviderRequestApprenticeTrainingOuterApi outerApi)
        {
            _outerApi = outerApi;
        }

        public async Task<List<AggregatedEmployerRequest>> Handle(GetAggregatedEmployerRequestsQuery request, CancellationToken cancellationToken)
        {
            var aggregatedEmployerRequests = await _outerApi.GetAggregatedEmployerRequests();

            return aggregatedEmployerRequests;
        }
    }
}
