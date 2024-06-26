using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Types;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models
{
    public class GetViewAggregatedEmployerRequestsViewModel
    {
        public List<AggregatedEmployerRequest> AggregatedEmployerRequests { get; set; }

        public int RequestCount { get { return AggregatedEmployerRequests.Count; } }
    }
}
