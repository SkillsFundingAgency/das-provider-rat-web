using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.SelectEmployerRequests;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators
{
    public interface IEmployerRequestOrchestrator
    {
        Task<ActiveEmployerRequestsViewModel> GetActiveEmployerRequestsViewModel(long ukprn);

        Task<SelectEmployerRequestsViewModel> GetSelectEmployerRequestsViewModel(long ukprn, string standardReference);

        void UpdateSelectedRequests(SelectedRequestsViewModel viewModel);
    }
}
