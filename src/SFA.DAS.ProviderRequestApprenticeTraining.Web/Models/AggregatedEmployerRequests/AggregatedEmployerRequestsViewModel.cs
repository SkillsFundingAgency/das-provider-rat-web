using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models
{
    public class AggregatedEmployerRequestsViewModel
    {
        public List<AggregatedEmployerRequestViewModel> AggregatedEmployerRequests { get; set; }

        public int RequestCount { get { return AggregatedEmployerRequests.Count; } }
    }
}
