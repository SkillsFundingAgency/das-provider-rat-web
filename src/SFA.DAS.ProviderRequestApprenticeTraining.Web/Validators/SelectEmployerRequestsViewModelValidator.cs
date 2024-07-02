using FluentValidation;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Validators
{
    public class SelectEmployerRequestsViewModelValidator : AbstractValidator<SelectEmployerRequestsViewModel>
    {
        public SelectEmployerRequestsViewModelValidator()
        {
            RuleFor(x => x.SelectedRequests)
                .ValidateSelectedRequests();
        }
    }

    public static class SelectEmployerRequestsViewModelValidatorRules
    {
        public static IRuleBuilderOptions<T, List<Guid>> ValidateSelectedRequests<T>(this IRuleBuilder<T, List<Guid>> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                    .WithMessage("You must select a request");
        }
    }
}
