using MediatR;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds
{
    public class GetCheckYourAnswersQuery : IRequest<GetCheckYourAnswersResult>
    {
        public List<Guid> EmployerRequestIds { get; set; }
        public long Ukprn { get; set; }

        public GetCheckYourAnswersQuery(long ukprn, List<Guid> employerRequestIds)
        {
            EmployerRequestIds = employerRequestIds;
            Ukprn = ukprn;
        }
    }
}
