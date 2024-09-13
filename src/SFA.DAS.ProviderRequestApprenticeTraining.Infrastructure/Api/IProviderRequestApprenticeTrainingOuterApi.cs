using RestEase;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Types;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Requests;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces
{
    public interface IProviderRequestApprenticeTrainingOuterApi
    {
        [Get("/provider-responses/{providerResponseId}/confirmation")]
        Task<GetProviderResponseConfirmationResponse> GetProviderResponseConfirmation([Path] Guid providerResponseId);

        [Post("/providers/{ukprn}/responses")]
        Task<SubmitProviderResponseResponse> SubmitProviderResponse([Path]long ukprn, [Body] SubmitProviderResponseRequest request);

        [Get("/providers/{ukprn}/check-answers")]
        Task<GetCheckYourAnswersResponse> GetCheckYourAnswers([Path]long ukprn, [Query]List<Guid> employerRequestids);

        [Get("/providers/{ukprn}/phone-numbers")]
        Task<GetProviderPhoneNumbersResponse> GetProviderPhoneNumbers([Path] long ukprn);

        [Get("/providers/{ukprn}/email-addresses")]
        Task<GetProviderEmailResponse> GetProviderEmailAddresses([Path] long ukprn, [Query] string userEmailAddress);

        [Post("/providers/{ukprn}/employer-requests/acknowledge")]
        Task CreateProviderResponse([Path]long ukprn, [Body]CreateProviderResponseEmployerRequestRequest request);

        [Get("/providers/{ukprn}/active")]
        Task<List<AggregatedEmployerRequestResponse>> GetAggregatedEmployerRequests([Path]long ukprn);

        [Get("/providers/{ukprn}/employer-requests/{standardReference}/select")]
        Task<SelectEmployerRequestsResponse> GetSelectEmployerRequests([Path]long ukprn, [Path]string standardReference);

        [Get("/provideraccounts/{ukprn}")]
        Task<ProviderAccountResponse> GetProviderDetails([Path] int ukprn);

        [Get("/ping")]
        Task Ping();
    }
}
