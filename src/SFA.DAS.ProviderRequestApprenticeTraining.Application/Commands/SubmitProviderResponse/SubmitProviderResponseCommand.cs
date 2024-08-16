using MediatR;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.SubmitProviderResponse
{
    public class SubmitProviderResponseCommand : IRequest<SubmitProviderResponseResult>
    {
        public long Ukprn { get; set; }
        public List<Guid> EmployerRequestIds { get; set; } = new List<Guid>();
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
    }
}
