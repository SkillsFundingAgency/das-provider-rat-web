using MediatR;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation
{
    public class GetProviderResponseConfirmationQuery : IRequest<GetProviderResponseConfirmationResult>
    {
        public Guid ProviderResponseId { get; set; }

        public GetProviderResponseConfirmationQuery(Guid providerResponseId)
        {
            ProviderResponseId = providerResponseId;
        }
    }
}
