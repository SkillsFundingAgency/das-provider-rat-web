using System;
namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses
{
    public class SelectEmployerRequestsResponse
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }

        public List<SelectEmployerRequestResponse> EmployerRequests { get; set; }
    }
}
