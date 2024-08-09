using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses
{
    public class EmployerRequestsByIdsResponse
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public List<EmployerRequestResponse> EmployerRequests { get; set; }
    }

    public class EmployerRequestResponse
    {
        public Guid EmployerRequestId { get; set; }
        public List<string> Locations { get; set; }
        public List<string> DeliveryMethods { get; set; }
        public string DateOfRequest { get; set; }
        public int NumberOfApprentices { get; set; }
    }
}
