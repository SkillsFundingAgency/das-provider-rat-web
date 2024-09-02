using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmails;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest
{
    public class SelectProviderEmailViewModel:EmployerRequestsParameters
    {
        public List<string> EmailAddresses { get; set; }
        public string SelectedEmail { get; set; }
        public bool HasSingleEmail { get; set; }
        public override string BackRoute
        {
            get
            {
                if (BackToCheckAnswers) return EmployerRequestController.CheckYourAnswersRouteGet;
                return EmployerRequestController.SelectRequestsToContactRouteGet;
            }
        }

        public static implicit operator SelectProviderEmailViewModel(GetProviderEmailsResult source)
        {
            return new SelectProviderEmailViewModel
            {
                EmailAddresses = source.EmailAddresses,
            };
        }
    }
}
