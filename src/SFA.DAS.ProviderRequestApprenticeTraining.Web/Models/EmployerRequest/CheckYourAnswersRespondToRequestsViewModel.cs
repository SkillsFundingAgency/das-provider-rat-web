using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest
{
    public class CheckYourAnswersRespondToRequestsViewModel : EmployerRequestsParameters
    {
        public string StandardLevel { get; set; }
        public string StandardTitle { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public bool HasSingleEmail { get; set; }
        public string Phone { get; set; }
        public bool HasSinglePhone { get; set; }
        public List<EmployerRequestViewModel> SelectedRequests { get; set; }
        public List<Guid> SelectedRequestIds { get; set; }

        public override string BackRoute {
            get
            {
                if (!HasSinglePhone) return EmployerRequestController.SelectProviderPhoneRouteGet;
                if (!HasSingleEmail) return EmployerRequestController.SelectProviderEmailRoutePost;
                return EmployerRequestController.SelectRequestsToContactRouteGet; 
            } 
        }
    }
}
