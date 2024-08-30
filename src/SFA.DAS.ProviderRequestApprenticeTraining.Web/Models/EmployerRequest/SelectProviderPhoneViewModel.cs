using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderPhoneNumbers;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest
{
    public class SelectProviderPhoneViewModel:EmployerRequestsParameters
    {
        public List<string> PhoneNumbers { get; set; }
        public string SelectedPhoneNumber { get; set; }
        public bool HasSinglePhoneNumber { get; set; }
        public bool HasSingleEmail { get; set; }

        public override string BackRoute
        {
            get
            {
                if (BackToCheckAnswers) return EmployerRequestController.CheckYourAnswersRouteGet;
                if (!HasSingleEmail) return EmployerRequestController.SelectProviderEmailRouteGet;
                return EmployerRequestController.SelectRequestsToContactRouteGet;
            }
        }

        public static implicit operator SelectProviderPhoneViewModel(GetProviderPhoneNumbersResult source)
        {
            return new SelectProviderPhoneViewModel
            {
                PhoneNumbers = source.PhoneNumbers,
            };
        }
    }
}
