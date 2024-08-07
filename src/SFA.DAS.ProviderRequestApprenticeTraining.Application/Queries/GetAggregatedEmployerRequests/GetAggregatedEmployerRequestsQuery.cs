using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequests
{
    public class GetAggregatedEmployerRequestsQuery : IRequest<GetAggregatedEmployerRequestsResult>
    {
        public long Ukprn { get; set; }

        public GetAggregatedEmployerRequestsQuery(long ukprn)
        { 
            Ukprn = ukprn;
        }
    }
}
