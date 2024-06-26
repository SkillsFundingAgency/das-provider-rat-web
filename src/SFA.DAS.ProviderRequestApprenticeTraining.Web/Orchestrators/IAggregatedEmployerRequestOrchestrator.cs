using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Web.Orchestrators
{
    public interface IAggregatedEmployerRequestOrchestrator
    {
        Task<GetViewAggregatedEmployerRequestsViewModel> GetViewAggregatedEmployerRequestsViewModel();
    }
}