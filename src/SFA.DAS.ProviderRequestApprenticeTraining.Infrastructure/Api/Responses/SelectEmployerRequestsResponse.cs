using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
