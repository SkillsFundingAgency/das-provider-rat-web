using RestEase;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Types;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Requests;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces
{
    public interface IProviderRequestApprenticeTrainingOuterApi
    {
        [Get("/employerrequests/providerresponse/{providerResponseId}/confirmation")]
        Task<GetProviderResponseConfirmationResponse> GetProviderResponseConfirmation([Path] Guid providerResponseId);

        [Post("/employerrequests/provider/{ukprn}/submit-response")]
        Task<SubmitProviderResponseResponse> SubmitProviderResponse([Path]long ukprn, [Body] SubmitProviderResponseRequest request);

        [Get("/employerrequests/provider/{ukprn}/check-answers")]
        Task<GetCheckYourAnswersResponse> GetCheckYourAnswers([Path]long ukprn, [Query]List<Guid> employerRequestids);

        [Get("/employerrequests/provider/{ukprn}/phonenumbers")]
        Task<GetProviderPhoneNumbersResponse> GetProviderPhoneNumbers([Path] long ukprn);

        [Get("/employerrequests/provider/{ukprn}/email-addresses")]
        Task<GetProviderEmailResponse> GetProviderEmailAddresses([Path] long ukprn, [Query] string userEmailAddress);

        [Post("/employerrequests/provider/{ukprn}/acknowledge-requests")]
        Task CreateProviderResponse([Path]long ukprn, [Body]CreateProviderResponseEmployerRequestRequest request);

        [Get("/employerrequests/provider/{ukprn}/active")]
        Task<List<AggregatedEmployerRequestResponse>> GetAggregatedEmployerRequests([Path]long ukprn);

        [Get("/employerrequests/provider/{ukprn}/selectrequests/{standardReference}")]
        Task<SelectEmployerRequestsResponse> GetSelectEmployerRequests([Path]long ukprn, [Path]string standardReference);

        [Get("/provideraccounts/{ukprn}")]
        Task<ProviderAccountResponse> GetProviderDetails([Path] int ukprn);

        [Get("/ping")]
        Task Ping();
    }
}
