using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests
{
    public class GetAggregatedEmployerRequestsResult
    {
        public List<AggregatedEmployerRequestResponse> AggregatedEmployerRequests { get; set; }
    }
}
