using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services
{
    public class TrainingProviderService : ITrainingProviderService
    {
        private readonly IProviderRequestApprenticeTrainingOuterApi _outerApi;

        public TrainingProviderService(IProviderRequestApprenticeTrainingOuterApi outerApi)
        {
            _outerApi = outerApi;
        }

        public async Task<bool> CanProviderAccessService(int ukprn)
        {
            var response = await _outerApi.GetProviderDetails(ukprn);
            return response.CanAccessService;
        }
    }
}
