using FluentValidation;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Validators
{
    public class SelectProviderEmailViewModelValidator : AbstractValidator<SelectProviderEmailViewModel>
    {
        public SelectProviderEmailViewModelValidator()
        {
            RuleFor(x => x.SelectedEmail)
                .ValidateSelectedEmail();
        }
    }

    public static class SelectedEmailViewModelValidatorRules
    {
        public static IRuleBuilderOptions<T, string> ValidateSelectedEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                    .WithMessage("Select an email address");
        }
    }
}
