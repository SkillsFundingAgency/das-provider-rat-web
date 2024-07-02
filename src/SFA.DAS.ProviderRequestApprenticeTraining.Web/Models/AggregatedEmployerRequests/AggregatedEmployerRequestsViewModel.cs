using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Infrastructure;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models
{
    public class AggregatedEmployerRequestsViewModel
    {
        public List<AggregatedEmployerRequestViewModel> AggregatedEmployerRequests { get; set; }

        public int RequestCount { get { return AggregatedEmployerRequests.Count; } }

        public string BackRoute 
        {
            get { return RouteNames.HomeGetStart; }
        }
    }
}
