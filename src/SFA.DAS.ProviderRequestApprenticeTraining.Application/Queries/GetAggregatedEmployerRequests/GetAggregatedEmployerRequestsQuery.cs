
using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Types;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequests
{
    public class GetAggregatedEmployerRequestsQuery : IRequest<List<AggregatedEmployerRequest>>
    {
    }
}
