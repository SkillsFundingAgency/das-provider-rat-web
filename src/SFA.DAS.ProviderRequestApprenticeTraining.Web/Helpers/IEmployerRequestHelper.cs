using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RestEase;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Helpers
{
    public static class IEmployerRequestExtensions
    {
        public static List<string> GetDeliveryMethods(this IEmployerRequestResponse model)
        {
            var methods = new List<string>();

            if (model.AtApprenticesWorkplace)
            {
                methods.Add("At apprentice's workplace");
            }
            if (model.BlockRelease)
            {
                methods.Add("Block release");
            }
            if (model.DayRelease)
            {
                methods.Add("Day release");
            }
            return methods;
        }
    }
}
