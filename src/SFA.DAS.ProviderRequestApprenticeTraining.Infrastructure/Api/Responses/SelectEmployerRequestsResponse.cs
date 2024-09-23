using System;
namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses
{
    public class SelectEmployerRequestsResponse
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }

        public int ExpiryAfterMonths { get; set; }
        public int RemovedAfterRequestedMonths { get; set; }

        public List<SelectEmployerRequestResponse> EmployerRequests { get; set; }
    }
}
