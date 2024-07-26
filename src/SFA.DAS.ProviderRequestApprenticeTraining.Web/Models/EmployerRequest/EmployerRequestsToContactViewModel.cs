using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest
{
    public class EmployerRequestsToContactViewModel
    {
        public long Ukprn { get; set; }
        public string StandardReference { get; set; }
        public List<Guid> MySelectedRequests { get; set; }
    }
}
