using MediatR;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmails
{
    public class GetProviderEmailsQuery : IRequest<GetProviderEmailsResult>
    {
        public long Ukprn { get; set; }
        public string UserEmailAddress { get; set; }

        public GetProviderEmailsQuery(long ukprn, string userEmailAddress)
        {
            Ukprn = ukprn;
            UserEmailAddress = userEmailAddress;
        }
    }
}
