using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models
{
    public class ActiveEmployerRequestsViewModel
    {
        public long Ukprn { get; set; }
        public string StandardReference { get; set; }

        public string BackLink { get; set; }

        public List<ActiveEmployerRequestViewModel> AggregatedEmployerRequests { get; set; } = new List<ActiveEmployerRequestViewModel>();

        public int RequestCount { get { return AggregatedEmployerRequests.Count; } }

    }
}
