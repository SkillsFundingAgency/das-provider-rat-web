using MediatR;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderPhoneNumbers
{
    public class GetProviderPhoneNumbersQuery : IRequest<GetProviderPhoneNumbersResult>
    {
        public long Ukprn { get; set; }

        public GetProviderPhoneNumbersQuery(long ukprn)
        {
            Ukprn = ukprn;
        }
    }
}
