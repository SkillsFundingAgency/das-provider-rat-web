using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest
{
    public class EmployerRequestsToContactViewModel
    {
        public long Ukprn { get; set; }
        public string StandardReference { get; set; }
        public List<Guid> SelectedRequests { get; set; } = new List<Guid>();
        public List<ViewedEmployerRequestViewModel> ViewedEmployerRequests { get; set; } = new List<ViewedEmployerRequestViewModel>();
    }
    public class ViewedEmployerRequestViewModel
    {
        public Guid EmployerRequestId { get; set; }
        public bool IsNew { get; set; }

    }
}
