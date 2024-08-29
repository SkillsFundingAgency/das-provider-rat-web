using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses
{
    public class GetCheckYourAnswersResponse
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string Website { get; set; }
        public List<EmployerRequestResponse> Requests { get; set; }
    }

    public class EmployerRequestResponse : IEmployerRequestResponse
    {
        public Guid EmployerRequestId { get; set; }
        public List<string> Locations { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public bool DayRelease { get; set; }
        public bool BlockRelease { get; set; }
        public string DateOfRequest { get; set; }
        public int NumberOfApprentices { get; set; }
    }
}
