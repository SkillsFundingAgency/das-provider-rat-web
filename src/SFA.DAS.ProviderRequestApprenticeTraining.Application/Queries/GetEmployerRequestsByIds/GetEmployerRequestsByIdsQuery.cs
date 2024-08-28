using MediatR;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds
{
    public class GetEmployerRequestsByIdsQuery : IRequest<GetEmployerRequestsByIdsResult>
    {
        public List<Guid> EmployerRequestIds { get; set; }

        public GetEmployerRequestsByIdsQuery(List<Guid> employerRequestIds)
        {
            EmployerRequestIds = employerRequestIds;
        }
    }
}
