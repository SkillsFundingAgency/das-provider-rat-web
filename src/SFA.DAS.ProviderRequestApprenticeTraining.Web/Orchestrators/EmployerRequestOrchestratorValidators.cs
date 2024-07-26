using FluentValidation;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators
{
    [ExcludeFromCodeCoverage]
    public class EmployerRequestOrchestratorValidators
    {
        public IValidator<EmployerRequestsToContactViewModel> SelectedRequestsModelValidator { get; set; }
        //public IValidator<EnterSameLocationEmployerRequestViewModel> EnterSameLocationEmployerRequestViewModelValidator { get; set; }
        //public IValidator<EnterSingleLocationEmployerRequestViewModel> EnterSingleLocationEmployerRequestViewModelValidator { get; set; }
        //public IValidator<EnterMultipleLocationsEmployerRequestViewModel> EnterMultipleLocationsEmployerRequestViewModelValidator { get; set; }
        //public IValidator<EnterTrainingOptionsEmployerRequestViewModel> EnterTrainingOptionsEmployerRequestViewModelValidator { get; set; }
        //public IValidator<CheckYourAnswersEmployerRequestViewModel> CheckYourAnswersEmployerRequestViewModelValidator { get; set; }
    }
}