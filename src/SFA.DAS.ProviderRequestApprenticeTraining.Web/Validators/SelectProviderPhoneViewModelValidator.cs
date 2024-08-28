using FluentValidation;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Validators
{
    public class SelectProviderPhoneViewModelValidator : AbstractValidator<SelectProviderPhoneViewModel>
    {
        public SelectProviderPhoneViewModelValidator()
        {
            RuleFor(x => x.SelectedPhoneNumber)
                .ValidateSelectedPhone();
        }
    }

    public static class SelectedProviderPhoneViewModelValidatorRules
    {
        public static IRuleBuilderOptions<T, string> ValidateSelectedPhone<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                    .WithMessage("Select a telephone number");
        }
    }
}
