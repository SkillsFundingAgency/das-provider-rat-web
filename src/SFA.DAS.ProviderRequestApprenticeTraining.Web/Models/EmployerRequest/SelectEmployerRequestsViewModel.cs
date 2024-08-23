using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest
{
    public class SelectEmployerRequestsViewModel : EmployerRequestsParameters
    {
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public List<SelectEmployerRequestViewModel> AllEmployerRequests { get; set; }
        public List<Guid> SelectedRequests { get; set; }

        public static implicit operator SelectEmployerRequestsViewModel(GetSelectEmployerRequestsResult source)
        {
            return new SelectEmployerRequestsViewModel
            {
                StandardReference = source.SelectEmployerRequestsResponse.StandardReference,
                StandardTitle = source.SelectEmployerRequestsResponse.StandardTitle,
                StandardLevel = source.SelectEmployerRequestsResponse.StandardLevel,
                AllEmployerRequests = source.SelectEmployerRequestsResponse.EmployerRequests
                    .Select(request => (SelectEmployerRequestViewModel)request).ToList(),
            };
        }
    }
}
