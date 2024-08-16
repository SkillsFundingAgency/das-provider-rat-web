using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation
{
    public class GetProviderResponseConfirmationResult
    {
        public long Ukprn { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public List<EmployerRequestResponse> EmployerRequests { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
    }
}
