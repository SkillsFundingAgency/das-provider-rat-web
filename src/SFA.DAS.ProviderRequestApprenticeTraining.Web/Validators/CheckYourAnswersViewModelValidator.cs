using FluentValidation;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Validators
{
    public class CheckYourAnswersViewModelValidator : AbstractValidator<CheckYourAnswersRespondToRequestsViewModel>
    {
        public CheckYourAnswersViewModelValidator()
        {
            RuleFor(x => x.Email)
                .ValidateSelectedEmail();

            RuleFor(x => x.Phone)
                .ValidateSelectedPhone();

            RuleFor(x => x.SelectedRequests) 
                .ValidateSelectedRequests();

        }
    }

    public static class CheckYourAnswersRespondToRequestsViewModelValidatorRules
    {
        public static IRuleBuilderOptions<T, List<EmployerRequestViewModel>> ValidateSelectedRequests<T>(this IRuleBuilder<T, List<EmployerRequestViewModel>> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                    .WithMessage("You must select a request");
        }
    }
}
