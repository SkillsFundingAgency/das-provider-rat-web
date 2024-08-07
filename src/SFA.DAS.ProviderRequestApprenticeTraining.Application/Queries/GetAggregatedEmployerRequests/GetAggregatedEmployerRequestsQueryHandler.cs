using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequests
{
    public class GetAggregatedEmployerRequestsQueryHandler : IRequestHandler<GetAggregatedEmployerRequestsQuery, GetAggregatedEmployerRequestsResult>
    {
        private readonly IProviderRequestApprenticeTrainingOuterApi _outerApi;

        public GetAggregatedEmployerRequestsQueryHandler(IProviderRequestApprenticeTrainingOuterApi outerApi)
        {
            _outerApi = outerApi;
        }

        public async Task<GetAggregatedEmployerRequestsResult> Handle(GetAggregatedEmployerRequestsQuery request, CancellationToken cancellationToken)
        {
            var aggregatedEmployerRequests = await _outerApi.GetAggregatedEmployerRequests(request.Ukprn);

            return new GetAggregatedEmployerRequestsResult
            {
                AggregatedEmployerRequests = aggregatedEmployerRequests
            };
        }
    }
}
