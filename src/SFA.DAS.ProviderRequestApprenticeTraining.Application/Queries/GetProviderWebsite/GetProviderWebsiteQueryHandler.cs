using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;


namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderWebsite
{
    public class GetProviderWebsiteQueryHandler : IRequestHandler<GetProviderWebsiteQuery, GetProviderWebsiteResult>
    {
        private readonly IProviderRequestApprenticeTrainingOuterApi _outerApi;

        public GetProviderWebsiteQueryHandler(IProviderRequestApprenticeTrainingOuterApi outerApi)
        {
            _outerApi = outerApi;
        }

        public async Task<GetProviderWebsiteResult> Handle(GetProviderWebsiteQuery request, CancellationToken cancellationToken)
        {
            var result = await _outerApi.GetProviderWebsite(request.Ukprn);

            return new GetProviderWebsiteResult
            {
                 Website = result.Website
            };
        }
    }
}
