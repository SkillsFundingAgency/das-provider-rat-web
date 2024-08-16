using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services.SessionStorage
{
    public interface ISessionStorageService
    {
        ProviderResponse ProviderResponse { get; set; }
    }
}
