namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Requests
{
    public class UpdateProviderResponseStatusRequest
    {
        public int ProviderResponseStatus { get; set; }
        public long Ukprn { get; set; }
        public List<Guid> EmployerRequestIds { get; set; }
    }
}
