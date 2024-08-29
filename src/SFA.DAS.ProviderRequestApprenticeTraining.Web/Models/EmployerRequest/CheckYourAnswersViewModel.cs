using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest
{
    public class CheckYourAnswersViewModel
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string Website { get; set; }
        public List<EmployerRequestViewModel> SelectedRequests { get; set; }

        public static implicit operator CheckYourAnswersViewModel(GetCheckYourAnswersResult source)
        {
            return new CheckYourAnswersViewModel
            {
                StandardReference = source.StandardReference,
                StandardTitle = source.StandardTitle,
                StandardLevel = source.StandardLevel,
                Website = source.Website,
                SelectedRequests = source.EmployerRequests
                    .Select(request => (EmployerRequestViewModel)request).ToList(),
            };
        }
    }

    public class EmployerRequestViewModel
    {
        public Guid EmployerRequestId { get; set; }
        public List<string> Locations { get; set; }
        public string DateOfRequest { get; set; }
        public int NumberOfApprentices { get; set; }
        public List<string> DeliveryMethods { get; set; }

        public static implicit operator EmployerRequestViewModel(EmployerRequestResponse source)
        {
            return new EmployerRequestViewModel
            {
                EmployerRequestId = source.EmployerRequestId,
                DateOfRequest = source.DateOfRequest,
                NumberOfApprentices = source.NumberOfApprentices,
                Locations = source.Locations,
                DeliveryMethods = source.GetDeliveryMethods(),
            };
        }
    }
}
