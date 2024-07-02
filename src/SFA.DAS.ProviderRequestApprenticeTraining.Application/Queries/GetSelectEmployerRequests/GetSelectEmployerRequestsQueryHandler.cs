using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;


namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests
{
    public class GetSelectEmployerRequestsQueryHandler : IRequestHandler<GetSelectEmployerRequestsQuery, GetSelectEmployerRequestsResult>
    {
        private readonly IProviderRequestApprenticeTrainingOuterApi _outerApi;

        public GetSelectEmployerRequestsQueryHandler(IProviderRequestApprenticeTrainingOuterApi outerApi)
        {
            _outerApi = outerApi;
        }

        public async Task<GetSelectEmployerRequestsResult> Handle(GetSelectEmployerRequestsQuery request, CancellationToken cancellationToken)
        {
            var result = await _outerApi.GetSelectEmployerRequests(request.StandardReference, request.Ukprn);

            return new GetSelectEmployerRequestsResult
            {
                SelectEmployerRequestsResponse = result,
            };
        }
    }
}
