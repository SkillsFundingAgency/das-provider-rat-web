using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds
{
    public class GetEmployerRequestsByIdsResult
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }

        public List<EmployerRequestResponse> EmployerRequests { get; set; }
    }
}
