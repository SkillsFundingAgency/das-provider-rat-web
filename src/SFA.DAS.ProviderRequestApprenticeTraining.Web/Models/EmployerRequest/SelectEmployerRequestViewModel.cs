using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Helpers;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest
{
    public class SelectEmployerRequestViewModel
    {
        public Guid EmployerRequestId { get; set; }
        public List<string> Locations { get; set; }
        public string DateOfRequest { get; set; }
        public int NumberOfApprentices { get; set; }
        public List<string> DeliveryMethods { get; set; }
        public bool IsNew { get; set; }
        public bool IsContacted { get; set; }
        public bool IsSelected { get; set; }

        public static implicit operator SelectEmployerRequestViewModel(SelectEmployerRequestResponse source)
        {
            return new SelectEmployerRequestViewModel
            {
                EmployerRequestId = source.EmployerRequestId,
                DateOfRequest = source.DateOfRequest,
                IsContacted = source.IsContacted,
                IsNew = source.IsNew,
                NumberOfApprentices = source.NumberOfApprentices,
                Locations = source.Locations,
                DeliveryMethods = source.GetDeliveryMethods(),
            };
        }
    }
}
