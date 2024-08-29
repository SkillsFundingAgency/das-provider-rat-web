using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Requests;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequest
{
    public class CreateProviderResponseEmployerRequestsCommandHandler : IRequestHandler<CreateProviderResponseEmployerRequestCommand>
    {
        private readonly IProviderRequestApprenticeTrainingOuterApi _outerApi;

        public CreateProviderResponseEmployerRequestsCommandHandler(IProviderRequestApprenticeTrainingOuterApi outerApi)
        {
            _outerApi = outerApi;
        }

        public async Task Handle(CreateProviderResponseEmployerRequestCommand command, CancellationToken cancellationToken)
        {
            await _outerApi.CreateProviderResponse(command.Ukprn, new CreateProviderResponseEmployerRequestRequest
            {
                EmployerRequestIds = command.EmployerRequestIds
            });

        }
    }
}
