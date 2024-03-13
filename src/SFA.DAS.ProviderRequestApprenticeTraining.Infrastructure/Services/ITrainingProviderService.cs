namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services
{
    public interface ITrainingProviderService
    {
        Task<bool> CanProviderAccessService(int ukprn);
    }
}
