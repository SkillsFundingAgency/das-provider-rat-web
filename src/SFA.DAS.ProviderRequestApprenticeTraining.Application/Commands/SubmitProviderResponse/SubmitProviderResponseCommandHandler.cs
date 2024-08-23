using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Requests;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.SubmitProviderResponse
{
    public class SubmitProviderResponseCommandHandler : IRequestHandler<SubmitProviderResponseCommand, SubmitProviderResponseResult>
    {
        private readonly IProviderRequestApprenticeTrainingOuterApi _outerApi;

        public SubmitProviderResponseCommandHandler(IProviderRequestApprenticeTrainingOuterApi outerApi)
        {
            _outerApi = outerApi;
        }

        public async Task<SubmitProviderResponseResult> Handle(SubmitProviderResponseCommand command, CancellationToken cancellationToken)
        {
            var response = await _outerApi.SubmitProviderResponse(new SubmitProviderResponseRequest
            {
                Ukprn = command.Ukprn,
                Email = command.Email,
                Phone = command.Phone,
                Website = command.Website,
                EmployerRequestIds = command.EmployerRequestIds,
                CurrentUserEmail = command.CurrentUserEmail,
                CurrentUserFirstName = command.CurrentUserFirstName,
                RespondedBy = command.RespondedBy,
                ContactName = command.ContactName,
            });

            return new SubmitProviderResponseResult { ProviderResponseId = response.ProviderResponseId };
        }
    }
}
