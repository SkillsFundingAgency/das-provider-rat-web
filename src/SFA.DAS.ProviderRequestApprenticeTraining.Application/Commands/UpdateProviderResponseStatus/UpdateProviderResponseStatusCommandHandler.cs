using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Requests;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.UpdateProviderResponseStatus
{
    public class UpdateProviderResponseStatusCommandHandler : IRequestHandler<UpdateProviderResponseStatusCommand, bool>
    {
        private readonly IProviderRequestApprenticeTrainingOuterApi _outerApi;

        public UpdateProviderResponseStatusCommandHandler(IProviderRequestApprenticeTrainingOuterApi outerApi)
        {
            _outerApi = outerApi;
        }

        public async Task<bool> Handle(UpdateProviderResponseStatusCommand command, CancellationToken cancellationToken)
        {
            var updateResult = await _outerApi.UpdateProviderResponseStatus(new UpdateProviderResponseStatusRequest
            {
                Ukprn = command.Ukprn,
                EmployerRequestIds = command.EmployerRequestIds,
                ProviderResponseStatus = command.ProviderResponseStatus,
            });

            return updateResult;
        }
    }
}
