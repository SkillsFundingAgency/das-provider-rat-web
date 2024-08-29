using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;


namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds
{
    public class GetCheckYourAnswersQueryHandler : IRequestHandler<GetCheckYourAnswersQuery, GetCheckYourAnswersResult>
    {
        private readonly IProviderRequestApprenticeTrainingOuterApi _outerApi;

        public GetCheckYourAnswersQueryHandler(IProviderRequestApprenticeTrainingOuterApi outerApi)
        {
            _outerApi = outerApi;
        }

        public async Task<GetCheckYourAnswersResult> Handle(GetCheckYourAnswersQuery request, CancellationToken cancellationToken)
        {
            var result = await _outerApi.GetCheckYourAnswers(request.Ukprn, request.EmployerRequestIds);

            return new GetCheckYourAnswersResult
            {
                StandardLevel = result.StandardLevel,
                StandardReference = result.StandardReference,
                StandardTitle = result.StandardTitle,
                Website = result.Website,
                EmployerRequests = result.Requests
            };
        }
    }
}
