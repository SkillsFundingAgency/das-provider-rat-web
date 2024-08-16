namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Requests
{
    public class CreateProviderResponseEmployerRequestRequest
    {
        public long Ukprn { get; set; }
        public List<Guid> EmployerRequestIds { get; set; }
    }
}
