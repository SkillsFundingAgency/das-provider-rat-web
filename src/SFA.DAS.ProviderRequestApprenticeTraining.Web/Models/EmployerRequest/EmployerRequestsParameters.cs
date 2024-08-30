using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest
{
    public class EmployerRequestsParameters
    {
        public long Ukprn { get; set; }
        public string StandardReference { get; set; }
        public virtual string BackRoute { get; set; }
        public bool BackToCheckAnswers { get; set; }
    }
}
