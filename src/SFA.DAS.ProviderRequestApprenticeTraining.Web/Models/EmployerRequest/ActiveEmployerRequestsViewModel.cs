﻿using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models
{
    public class ActiveEmployerRequestsViewModel
    {
        public long Ukprn { get; set; }

        public List<ActiveEmployerRequestViewModel> AggregatedEmployerRequests { get; set; }

        public int RequestCount { get { return AggregatedEmployerRequests.Count; } }

        public string BackRoute 
        {
            get { return EmployerRequestController.ActiveRouteGet; }
        }
    }
}
