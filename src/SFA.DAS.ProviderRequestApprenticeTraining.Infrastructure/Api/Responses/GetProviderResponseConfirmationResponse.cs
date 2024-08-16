namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses
{
    public class GetProviderResponseConfirmationResponse
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
