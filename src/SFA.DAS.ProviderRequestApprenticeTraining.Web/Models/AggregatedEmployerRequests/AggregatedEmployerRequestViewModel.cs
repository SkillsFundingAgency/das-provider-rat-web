using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Types;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models
{
    public class AggregatedEmployerRequestViewModel
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string StandardSector { get; set; }
        public int NumberOfApprentices { get; set; }
        public int NumberOfEmployers { get; set; }

        public static implicit operator AggregatedEmployerRequestViewModel(AggregatedEmployerRequestResponse source)
        {
            return new AggregatedEmployerRequestViewModel
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
