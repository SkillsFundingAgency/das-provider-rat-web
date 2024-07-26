using FluentValidation;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Validators
{
    public class SelectedRequestsViewModelValidator : AbstractValidator<EmployerRequestsToContactViewModel>
    {
        public SelectedRequestsViewModelValidator()
        {
            RuleFor(x => x.MySelectedRequests)
                .ValidateSelectedRequests();
        }
    }

    public static class SelectedRequestsViewModelValidatorRules
    {
        public static IRuleBuilderOptions<T, List<Guid>> ValidateSelectedRequests<T>(this IRuleBuilder<T, List<Guid>> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                    .WithMessage("You must select a request");
        }
    }
}
