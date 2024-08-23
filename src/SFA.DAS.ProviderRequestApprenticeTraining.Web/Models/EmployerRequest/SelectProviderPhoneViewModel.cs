using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderPhoneNumbers;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest
{
    public class SelectProviderPhoneViewModel:EmployerRequestsParameters
    {
        public List<string> PhoneNumbers { get; set; }
        public string SelectedPhoneNumber { get; set; }
        public bool HasSinglePhoneNumber { get; set; }

        public static implicit operator SelectProviderPhoneViewModel(GetProviderPhoneNumbersResult source)
        {
            return new SelectProviderPhoneViewModel
            {
                PhoneNumbers = source.PhoneNumbers,
            };
        }
    }
}
