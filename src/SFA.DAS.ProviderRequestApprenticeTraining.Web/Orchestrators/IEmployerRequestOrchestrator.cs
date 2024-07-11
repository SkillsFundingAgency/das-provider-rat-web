using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators
{
    public interface IEmployerRequestOrchestrator
    {
        Task<ActiveEmployerRequestsViewModel> GetActiveEmployerRequestsViewModel();
    }
}
