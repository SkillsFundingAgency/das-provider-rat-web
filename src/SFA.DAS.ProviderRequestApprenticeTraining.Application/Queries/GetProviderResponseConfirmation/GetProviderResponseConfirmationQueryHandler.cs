using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;


namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation
{
    public class GetProviderResponseConfirmationQueryHandler : IRequestHandler<GetProviderResponseConfirmationQuery, GetProviderResponseConfirmationResult>
    {
        private readonly IProviderRequestApprenticeTrainingOuterApi _outerApi;

        public GetProviderResponseConfirmationQueryHandler(IProviderRequestApprenticeTrainingOuterApi outerApi)
        {
            _outerApi = outerApi;
        }

        public async Task<GetProviderResponseConfirmationResult> Handle(GetProviderResponseConfirmationQuery request, CancellationToken cancellationToken)
        {
            var result = await _outerApi.GetProviderResponseConfirmation(request.ProviderResponseId);

            return new GetProviderResponseConfirmationResult
            {
                Email = result.Email,
                EmployerRequests = result.EmployerRequests,
                Phone = result.Phone,
                StandardLevel = result.StandardLevel,
                StandardTitle = result.StandardTitle,
                Ukprn = result.Ukprn,   
                Website = result.Website
            };
        }
    }
}
