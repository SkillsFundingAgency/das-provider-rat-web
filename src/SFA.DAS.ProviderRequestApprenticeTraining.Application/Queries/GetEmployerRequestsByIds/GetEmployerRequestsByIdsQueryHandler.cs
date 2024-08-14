using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;


namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds
{
    public class GetEmployerRequestsByIdsQueryHandler : IRequestHandler<GetEmployerRequestsByIdsQuery, GetEmployerRequestsByIdsResult>
    {
        private readonly IProviderRequestApprenticeTrainingOuterApi _outerApi;

        public GetEmployerRequestsByIdsQueryHandler(IProviderRequestApprenticeTrainingOuterApi outerApi)
        {
            _outerApi = outerApi;
        }

        public async Task<GetEmployerRequestsByIdsResult> Handle(GetEmployerRequestsByIdsQuery request, CancellationToken cancellationToken)
        {
            var result = await _outerApi.GetEmployerRequestsByIds(request.EmployerRequestIds);

            return new GetEmployerRequestsByIdsResult
            {
                StandardLevel = result.StandardLevel,
                StandardReference = result.StandardReference,
                StandardTitle = result.StandardTitle,
                EmployerRequests = result.Requests
            };
        }
    }
}
