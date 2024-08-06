using MediatR;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmails
{
    public class GetProviderEmailsQuery : IRequest<GetProviderEmailsResult>
    {
        public string StandardReference { get; set; }
        public long Ukprn { get; set; }

        public GetProviderEmailsQuery(long ukprn)
        {
            Ukprn = ukprn;
        }
    }
}
