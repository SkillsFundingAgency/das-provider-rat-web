using FluentValidation;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators
{
    [ExcludeFromCodeCoverage]
    public class EmployerRequestOrchestratorValidators
    {
        public IValidator<EmployerRequestsToContactViewModel> SelectedRequestsModelValidator { get; set; }
        public IValidator<SelectProviderEmailViewModel> SelectProviderEmailViewModelValidator { get; set; }
        public IValidator<SelectProviderPhoneViewModel> SelectProviderPhoneViewModelValidator { get; set; }

        public IValidator<CheckYourAnswersRespondToRequestsViewModel> CheckYourAnswersViewModelValidator { get; set; }
    }
}