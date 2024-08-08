using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services.SessionStorage
{
    public class ProviderResponse
    {
        public long Ukprn { get; set; }
        public List<Guid> SelectedEmployerRequests { get; set; } = new List<Guid>();
        public string SelectedEmail { get; set; }
        public bool HasSingleEmail { get; set; }
    }
}
