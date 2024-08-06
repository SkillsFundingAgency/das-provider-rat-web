using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models
{
    public class ActiveEmployerRequestsViewModel: EmployerRequestsParameters
    {
        public string BackLink { get; set; }

        public List<ActiveEmployerRequestViewModel> AggregatedEmployerRequests { get; set; } = new List<ActiveEmployerRequestViewModel>();

        public int RequestCount { get { return AggregatedEmployerRequests?.Count ?? 0; } }


    }
}
