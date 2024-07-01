

using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Types;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

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
            var aggregatedEmployerRequests = await _outerApi.GetAggregatedEmployerRequests();

            return new GetAggregatedEmployerRequestsResult
            {
                AggregatedEmployerRequests = aggregatedEmployerRequests
            };
        }
    }
}
