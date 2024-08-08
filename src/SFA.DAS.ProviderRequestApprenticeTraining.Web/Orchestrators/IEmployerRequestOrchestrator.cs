﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators
{
    public interface IEmployerRequestOrchestrator
    {
        void StartProviderResponse(long ukprn);
        Task<ActiveEmployerRequestsViewModel> GetActiveEmployerRequestsViewModel(long ukprn);
        Task<SelectEmployerRequestsViewModel> GetEmployerRequestsByStandardViewModel(EmployerRequestsParameters parameters, ModelStateDictionary modelState);
        Task<bool> ValidateEmployerRequestsToContactViewModel(EmployerRequestsToContactViewModel viewModel, ModelStateDictionary modelState);
        Task UpdateSelectedRequests(EmployerRequestsToContactViewModel viewModel);

        Task<SelectProviderEmailViewModel> GetProviderEmailsViewModel(GetProviderEmailsParameters parameters, ModelStateDictionary modelState);
        Task<bool> ValidateProviderEmailsViewModel(SelectProviderEmailViewModel viewModel, ModelStateDictionary modelState);
        void UpdateProviderEmail(SelectProviderEmailViewModel viewModel);

        void ClearProviderResponse();
    }
}
