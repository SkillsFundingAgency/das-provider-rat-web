using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models
{
    public class ActiveEmployerRequestViewModel
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string StandardSector { get; set; }
        public int NumberOfApprentices { get; set; }
        public int NumberOfEmployers { get; set; }

        public static implicit operator ActiveEmployerRequestViewModel(AggregatedEmployerRequestResponse source)
        {
            return new ActiveEmployerRequestViewModel
            {
                NumberOfApprentices = source.NumberOfApprentices,
                StandardLevel = source.StandardLevel,   
                StandardSector = source.StandardSector,
                NumberOfEmployers = source.NumberOfEmployers,
                StandardReference = source.StandardReference,
                StandardTitle = source.StandardTitle,
            };
        }
    }
}
