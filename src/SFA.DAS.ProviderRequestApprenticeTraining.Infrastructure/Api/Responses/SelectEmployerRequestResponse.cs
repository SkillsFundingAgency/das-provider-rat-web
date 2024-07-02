using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses
{
    public class SelectEmployerRequestResponse
    {
        public Guid EmployerRequestId { get; set; }
        public List<string> Locations { get; set; }
        public List<string> DeliveryMethods { get; set; }
        public string DateOfRequest { get; set; }
        public int NumberOfApprentices { get; set; }
        public bool IsNew { get; set; }
        public bool IsContacted { get; set; }
    }
}
