using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;


namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmails
{
    public class GetProviderEmailsQueryHandler : IRequestHandler<GetProviderEmailsQuery, GetProviderEmailsResult>
    {
        private readonly IProviderRequestApprenticeTrainingOuterApi _outerApi;

        public GetProviderEmailsQueryHandler(IProviderRequestApprenticeTrainingOuterApi outerApi)
        {
            _outerApi = outerApi;
        }

        public async Task<GetProviderEmailsResult> Handle(GetProviderEmailsQuery request, CancellationToken cancellationToken)
        {
            var result = await _outerApi.GetProviderEmailAddresses(request.Ukprn, request.UserEmailAddress);

            return new GetProviderEmailsResult
            {
                 EmailAddresses = result.EmailAddresses,
            };
        }
    }
}
