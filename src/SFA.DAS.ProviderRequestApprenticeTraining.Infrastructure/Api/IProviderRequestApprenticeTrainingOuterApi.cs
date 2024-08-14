using RestEase;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Types;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Requests;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces
{
    public interface IProviderRequestApprenticeTrainingOuterApi
    {
        [Get("/employerrequests")]
        Task<EmployerRequestsByIdsResponse> GetEmployerRequestsByIds([Query]List<Guid> employerRequestids);

        [Get("/employerrequests/provider/{ukprn}/website")]
        Task<GetProviderWebsiteResponse> GetProviderWebsite([Path] long ukprn);

        [Get("/employerrequests/provider/{ukprn}/phonenumbers")]
        Task<GetProviderPhoneNumbersResponse> GetProviderPhoneNumbers([Path] long ukprn);

        [Get("/employerrequests/provider/{ukprn}/email-addresses")]
        Task<GetProviderEmailResponse> GetProviderEmailAddresses([Path] long ukprn, [Query] string userEmailAddress);

        [Post("/employerrequests/provider/responses")]
        Task CreateProviderResponse([Body]CreateProviderResponseEmployerRequestRequest request);

        [Get("/employerrequests/provider/{ukprn}/aggregated")]
        Task<List<AggregatedEmployerRequestResponse>> GetAggregatedEmployerRequests([Path]long ukprn);

        [Get("/employerrequests/provider/{ukprn}/selectrequests/{standardReference}")]
        Task<SelectEmployerRequestsResponse> GetSelectEmployerRequests([Path]long ukprn, [Path]string standardReference);

        [Get("/employerrequests/{employerRequestId}")]
        Task<EmployerRequest> GetEmployerRequest([Path] Guid employerRequestId);

        [Get("/employerrequests/requesttype/{requestType}")]
        Task<List<EmployerRequest>> GetEmployerRequests([Path] string requestType);

        [Get("/provideraccounts/{ukprn}")]
        Task<ProviderAccountResponse> GetProviderDetails([Path] int ukprn);

        [Get("/ping")]
        Task Ping();
    }
}
