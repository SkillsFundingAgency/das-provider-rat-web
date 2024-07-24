using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models
{
    public class ActiveEmployerRequestsViewModel
    {
        public List<ActiveEmployerRequestViewModel> AggregatedEmployerRequests { get; set; }

        public int RequestCount { get { return AggregatedEmployerRequests.Count; } }

        public string BackLink { get; set; }
    }
}
