﻿using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services.SessionStorage
{
    public interface ISessionStorageService
    {
        MySessionObject MySessionObject { get; set; }
    }
}
