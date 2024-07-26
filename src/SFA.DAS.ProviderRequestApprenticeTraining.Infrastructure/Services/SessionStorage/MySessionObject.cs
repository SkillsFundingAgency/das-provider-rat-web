using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services.SessionStorage
{
    public class MySessionObject
    {
        public List<Guid> SelectedEmployerRequests { get; set; } = new List<Guid>();
    }
}
