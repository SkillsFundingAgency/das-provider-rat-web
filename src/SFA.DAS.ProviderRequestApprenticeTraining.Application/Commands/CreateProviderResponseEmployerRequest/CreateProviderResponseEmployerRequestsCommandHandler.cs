using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Requests;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequest
{
    public class CreateProviderResponseEmployerRequestsCommandHandler : IRequestHandler<CreateProviderResponseEmployerRequestCommand, bool>
    {
        private readonly IProviderRequestApprenticeTrainingOuterApi _outerApi;

        public CreateProviderResponseEmployerRequestsCommandHandler(IProviderRequestApprenticeTrainingOuterApi outerApi)
        {
            _outerApi = outerApi;
        }

        public async Task<bool> Handle(CreateProviderResponseEmployerRequestCommand command, CancellationToken cancellationToken)
        {
            var updateResult = await _outerApi.UpdateProviderResponseStatus(new CreateProviderResponseEmployerRequestRequest
            {
                Ukprn = command.Ukprn,
                EmployerRequestIds = command.EmployerRequestIds
            });

            return updateResult;
        }
    }
}
