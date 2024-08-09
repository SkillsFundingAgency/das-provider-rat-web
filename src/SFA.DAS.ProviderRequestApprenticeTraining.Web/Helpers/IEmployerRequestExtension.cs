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
        public static List<string> GetDeliveryMethods(this SelectEmployerRequestResponse model)
        {
            var methods = new List<string>();

            if (model.AtApprenticesWorkplace)
            {
                methods.Add("At Apprentices Workplace");
            }
            if (model.BlockRelease)
            {
                methods.Add("Block Release");
            }
            if (model.DayRelease)
            {
                methods.Add("Day Release");
            }
            return methods;
        }
    }
}
