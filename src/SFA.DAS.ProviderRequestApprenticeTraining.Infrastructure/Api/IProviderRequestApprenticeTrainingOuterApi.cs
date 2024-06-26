using RestEase;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Types;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces
{
    public interface IProviderRequestApprenticeTrainingOuterApi
    {
        [Get("/employerrequest/aggregated-employer-requests")]
        Task<List<AggregatedEmployerRequest>> GetAggregatedEmployerRequests();

        [Get("/employerrequest/{employerRequestId}")]
        Task<EmployerRequest> GetEmployerRequest([Path] Guid employerRequestId);

        [Get("/employerrequest/requesttype/{requestType}")]
        Task<List<EmployerRequest>> GetEmployerRequests([Path] string requestType);

        [Get("/provideraccounts/{ukprn}")]
        Task<ProviderAccountResponse> GetProviderDetails([Path] int ukprn);

        [Get("/ping")]
        Task Ping();
    }
}
