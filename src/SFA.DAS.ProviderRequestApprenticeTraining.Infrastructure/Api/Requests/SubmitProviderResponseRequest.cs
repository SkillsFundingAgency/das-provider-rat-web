using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Requests
{
    public class SubmitProviderResponseRequest
    {
        public long Ukprn { get; set; }
        public List<Guid> EmployerRequestIds { get; set; } = new List<Guid>();
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string CurrentUserEmail { get; set; }
        public string CurrentUserFirstName { get; set; }
        public string RespondedBy { get; set; }
    }
}
