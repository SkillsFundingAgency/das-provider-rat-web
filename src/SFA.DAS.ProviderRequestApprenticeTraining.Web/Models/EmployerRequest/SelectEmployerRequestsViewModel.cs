using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;
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
        public List<SelectEmployerRequestViewModel> ContactedEmployerRequests { get; set; }
        public List<SelectEmployerRequestViewModel> NotContactedEmployerRequests { get; set; }
        public List<Guid> SelectedRequests { get; set; }
        public int TotalRequests { get; set; }

        public int ExpiryAfterMonths { get; set; }
        public int RemovedAfterRequestedMonths { get; set; }

        public override string BackRoute
        {
            get
            {
                if (BackToCheckAnswers) return EmployerRequestController.CheckYourAnswersRouteGet;
                return EmployerRequestController.ActiveRouteGet;
            }
        }

        public static implicit operator SelectEmployerRequestsViewModel(GetSelectEmployerRequestsResult source)
        {
            return new SelectEmployerRequestsViewModel
            {
                StandardReference = source.SelectEmployerRequestsResponse.StandardReference,
                StandardTitle = source.SelectEmployerRequestsResponse.StandardTitle,
                StandardLevel = source.SelectEmployerRequestsResponse.StandardLevel,
                ContactedEmployerRequests = source.SelectEmployerRequestsResponse.EmployerRequests
                    .Where(r => r.IsContacted)
                    .Select(request => (SelectEmployerRequestViewModel)request).ToList(),
                NotContactedEmployerRequests = source.SelectEmployerRequestsResponse.EmployerRequests
                    .Where(r => !r.IsContacted)
                    .Select(request => (SelectEmployerRequestViewModel)request).ToList(),
                TotalRequests = source.SelectEmployerRequestsResponse.EmployerRequests.Count
            };
        }
    }
}
