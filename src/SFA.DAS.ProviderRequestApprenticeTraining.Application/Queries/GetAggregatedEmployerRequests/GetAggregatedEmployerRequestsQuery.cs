
using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Types;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests
{
    public class GetAggregatedEmployerRequestsQuery : IRequest<GetAggregatedEmployerRequestsResult>
    {
    }
}
