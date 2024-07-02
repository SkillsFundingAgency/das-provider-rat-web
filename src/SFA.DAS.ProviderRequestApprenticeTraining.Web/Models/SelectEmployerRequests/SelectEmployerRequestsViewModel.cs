using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models
{
    public class SelectEmployerRequestsViewModel
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public List<SelectEmployerRequestViewModel> SelectEmployerRequests { get; set; }
        public List<Guid> SelectedRequests { get; set; }
        public string BackRoute
        {
            get { return AggregatedEmployerRequestController.AggregatedEmployerRequestsRouteGet; }
        }


        public static implicit operator SelectEmployerRequestsViewModel(GetSelectEmployerRequestsResult source)
        {
            return new SelectEmployerRequestsViewModel
            {
                StandardReference = source.SelectEmployerRequestsResponse.StandardReference,
                StandardTitle = source.SelectEmployerRequestsResponse.StandardTitle,
                StandardLevel = source.SelectEmployerRequestsResponse.StandardLevel,
                SelectEmployerRequests = source.SelectEmployerRequestsResponse.EmployerRequests
                    .Select(request => (SelectEmployerRequestViewModel)request).ToList(),
                SelectedRequests = new List<Guid>()
            };
        }
    }
}
