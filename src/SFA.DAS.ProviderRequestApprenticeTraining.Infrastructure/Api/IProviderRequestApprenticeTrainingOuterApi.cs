using RestEase;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Types;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces
{
    public interface IProviderRequestApprenticeTrainingOuterApi
    {
        [Get("/employerrequests/provider/{ukprn}/aggregated")]
        Task<List<AggregatedEmployerRequestResponse>> GetAggregatedEmployerRequests([Path]long ukprn);
        [Get("/employerrequest/provider/{ukprn}/selectrequests/{standardReference}")]
        Task<SelectEmployerRequestsResponse> GetSelectEmployerRequests([Path]long ukprn, [Path]string standardReference);

        [Get("/employerrequest/{employerRequestId}")]
        Task<EmployerRequest> GetEmployerRequest([Path] Guid employerRequestId);

        [Get("/employerrequests/requesttype/{requestType}")]
        Task<List<EmployerRequest>> GetEmployerRequests([Path] string requestType);

        [Get("/provideraccounts/{ukprn}")]
        Task<ProviderAccountResponse> GetProviderDetails([Path] int ukprn);

        [Get("/ping")]
        Task Ping();
    }
}
