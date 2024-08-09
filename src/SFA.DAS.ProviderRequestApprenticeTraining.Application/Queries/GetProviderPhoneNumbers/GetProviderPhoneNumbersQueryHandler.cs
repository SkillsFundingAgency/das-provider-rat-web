using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;


namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderPhoneNumbers
{
    public class GetProviderPhoneNumbersQueryHandler : IRequestHandler<GetProviderPhoneNumbersQuery, GetProviderPhoneNumbersResult>
    {
        private readonly IProviderRequestApprenticeTrainingOuterApi _outerApi;

        public GetProviderPhoneNumbersQueryHandler(IProviderRequestApprenticeTrainingOuterApi outerApi)
        {
            _outerApi = outerApi;
        }

        public async Task<GetProviderPhoneNumbersResult> Handle(GetProviderPhoneNumbersQuery request, CancellationToken cancellationToken)
        {
            var result = await _outerApi.GetProviderPhoneNumbers(request.Ukprn);

            return new GetProviderPhoneNumbersResult
            {
                PhoneNumbers = result.PhoneNumbers
            };
        }
    }
}
