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
            var response = await _outerApi.SubmitProviderResponse(command.Ukprn, new SubmitProviderResponseRequest
            {
                Email = command.Email,
                Phone = command.Phone ?? string.Empty,
                Website = command.Website ?? string.Empty,
                EmployerRequestIds = command.EmployerRequestIds,
                CurrentUserEmail = command.CurrentUserEmail,
                CurrentUserFirstName = command.CurrentUserFirstName,
                RespondedBy = command.RespondedBy,
                ContactName = command.ContactName,
                StandardLevel = command.StandardLevel,
                StandardTitle = command.StandardTitle,
            });

            return new SubmitProviderResponseResult { ProviderResponseId = response.ProviderResponseId };
        }
    }
}
