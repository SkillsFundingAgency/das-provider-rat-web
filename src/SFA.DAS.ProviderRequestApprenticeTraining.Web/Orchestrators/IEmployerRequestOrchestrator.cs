using Microsoft.AspNetCore.Mvc.ModelBinding;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators
{
    public interface IEmployerRequestOrchestrator
    {
        Task<ActiveEmployerRequestsViewModel> GetActiveEmployerRequestsViewModel(long ukprn);

        Task<SelectEmployerRequestsViewModel> GetEmployerRequestsByStandardViewModel(EmployerRequestsParameters parameters, ModelStateDictionary modelState);
        Task<bool> ValidateEmployerRequestsToContactViewModel(EmployerRequestsToContactViewModel viewModel, ModelStateDictionary modelState);

        void UpdateSelectedRequests(EmployerRequestsToContactViewModel viewModel);
    }
}
