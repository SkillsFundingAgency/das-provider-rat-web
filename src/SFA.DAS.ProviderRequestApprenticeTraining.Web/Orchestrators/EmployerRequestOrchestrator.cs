using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequest;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmails;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderPhoneNumbers;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderWebsite;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services.SessionStorage;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Helpers;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators
{
    public class EmployerRequestOrchestrator : IEmployerRequestOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ISessionStorageService _sessionStorage;
        private readonly EmployerRequestOrchestratorValidators _employerRequestOrchestratorValidators;
        private readonly ProviderSharedUIConfiguration _providerSharedUIConfiguration;

        public EmployerRequestOrchestrator(IMediator mediator, ISessionStorageService sessionStorage,
            EmployerRequestOrchestratorValidators employerRequestOrchestratorValidators,
            IOptions<ProviderSharedUIConfiguration> sharedUIConfiguration)
        {
            _mediator = mediator;
            _sessionStorage = sessionStorage;
            _employerRequestOrchestratorValidators = employerRequestOrchestratorValidators;
            _providerSharedUIConfiguration = sharedUIConfiguration.Value;
        }

        public void StartProviderResponse(long ukprn)
        {
            _sessionStorage.ProviderResponse = new ProviderResponse
            {
                Ukprn = ukprn,
            };
        }

        public async Task<ActiveEmployerRequestsViewModel> GetActiveEmployerRequestsViewModel(long ukprn)
        {
            var result = await _mediator.Send(new GetAggregatedEmployerRequestsQuery(ukprn));

            var model = new ActiveEmployerRequestsViewModel()
            {
                Ukprn = ukprn,
                AggregatedEmployerRequests = result.AggregatedEmployerRequests.Select(request => (ActiveEmployerRequestViewModel)request).ToList(),
                BackLink = _providerSharedUIConfiguration.DashboardUrl + "account"
            };

            return model;
        }

        public async Task<SelectEmployerRequestsViewModel> GetEmployerRequestsByStandardViewModel(EmployerRequestsParameters parameters, ModelStateDictionary modelState)
        {
            var result = await _mediator.Send(new GetSelectEmployerRequestsQuery(parameters.Ukprn, parameters.StandardReference));

            var viewModel = (SelectEmployerRequestsViewModel)result;
            viewModel.Ukprn = parameters.Ukprn;
            viewModel.StandardReference = parameters.StandardReference;
            viewModel.SelectedRequests = SessionProviderResponse.SelectedEmployerRequests;
            return viewModel;
        }

        public async Task<bool> ValidateEmployerRequestsToContactViewModel(EmployerRequestsToContactViewModel viewModel, ModelStateDictionary modelState)
        {
            return await ValidateViewModel(_employerRequestOrchestratorValidators.SelectedRequestsModelValidator, viewModel, modelState);
        }

        public async Task UpdateSelectedRequests(EmployerRequestsToContactViewModel viewModel)
        {
            if (viewModel.ViewedEmployerRequests.Any(r => r.IsNew))
            {
                await _mediator.Send(new CreateProviderResponseEmployerRequestCommand
                {
                    Ukprn = viewModel.Ukprn,
                    EmployerRequestIds = viewModel.ViewedEmployerRequests.Where(r => r.IsNew).Select(r => r.EmployerRequestId).ToList(),
                });
            }

            UpdateSessionProviderResponse((model) =>
            {
                model.SelectedEmployerRequests = viewModel.SelectedRequests;
            });
        }

        public async Task<SelectProviderEmailViewModel> GetProviderEmailsViewModel(GetProviderEmailsParameters parameters, ModelStateDictionary modelState)
        {
            var result = await _mediator.Send(new GetProviderEmailsQuery(parameters.Ukprn, parameters.UserEmailAddress));

            var viewModel = (SelectProviderEmailViewModel)result;
            viewModel.Ukprn = parameters.Ukprn;
            viewModel.StandardReference = parameters.StandardReference;

            if (viewModel.EmailAddresses.Count == 1)
            {
                viewModel.SelectedEmail = viewModel.EmailAddresses.FirstOrDefault();
                viewModel.HasSingleEmail = true;
                UpdateProviderEmail(viewModel);
            }
            else
            {
                viewModel.SelectedEmail = SessionProviderResponse.SelectedEmail;
            }

            return viewModel;
        }

        public async Task<bool> ValidateProviderEmailsViewModel(SelectProviderEmailViewModel viewModel, ModelStateDictionary modelState)
        {
            return await ValidateViewModel(_employerRequestOrchestratorValidators.SelectProviderEmailViewModelValidator, viewModel, modelState);
        }
        public void UpdateProviderEmail(SelectProviderEmailViewModel viewModel)
        {
            UpdateSessionProviderResponse((model) =>
            {
                model.SelectedEmail = viewModel.SelectedEmail;
                model.HasSingleEmail = viewModel.HasSingleEmail;
            });
        }

        public async Task<SelectProviderPhoneViewModel> GetProviderPhoneNumbersViewModel(EmployerRequestsParameters parameters, ModelStateDictionary modelState)
        {
            var result = await _mediator.Send(new GetProviderPhoneNumbersQuery(parameters.Ukprn));

            var viewModel = (SelectProviderPhoneViewModel)result;
            viewModel.Ukprn = parameters.Ukprn;
            viewModel.StandardReference = parameters.StandardReference;
            viewModel.HasSingleEmail = SessionProviderResponse.HasSingleEmail;

            if (viewModel.PhoneNumbers.Count == 1)
            {
                viewModel.SelectedPhoneNumber = viewModel.PhoneNumbers.FirstOrDefault();
                viewModel.HasSinglePhoneNumber = true;
                UpdateProviderPhone(viewModel);
            }
            else
            {
                viewModel.SelectedPhoneNumber = SessionProviderResponse.SelectedPhoneNumber;
            }
            return viewModel;
        }

        public async Task<bool> ValidateProviderPhoneViewModel(SelectProviderPhoneViewModel viewModel, ModelStateDictionary modelState)
        {
            return await ValidateViewModel(_employerRequestOrchestratorValidators.SelectProviderPhoneViewModelValidator, viewModel, modelState);
        }
        
        public void UpdateProviderPhone(SelectProviderPhoneViewModel viewModel)
        {
            UpdateSessionProviderResponse((model) =>
            {
                model.SelectedPhoneNumber = viewModel.SelectedPhoneNumber;
                model.HasSinglePhoneNumber = viewModel.HasSinglePhoneNumber;
            });
        }
        
        private ProviderResponse SessionProviderResponse
        {
            get => _sessionStorage.ProviderResponse ?? new ProviderResponse();
        }

        private void UpdateSessionProviderResponse(Action<ProviderResponse> action)
        {
            var providerResponse = SessionProviderResponse;
            action(providerResponse);
            _sessionStorage.ProviderResponse = providerResponse;
        }

        public void ClearProviderResponse()
        {
            _sessionStorage.ProviderResponse = null;
        }

        public async Task<CheckYourAnswersRespondToRequestsViewModel> GetCheckYourAnswersRespondToRequestsViewModel(EmployerRequestsParameters parameters, ModelStateDictionary modelState)
        {
            var providerResponse = SessionProviderResponse;

            var result = await _mediator.Send(new GetProviderWebsiteQuery(parameters.Ukprn));

            var selectedRequests = (EmployerRequestsViewModel) await _mediator.Send(new GetEmployerRequestsByIdsQuery(providerResponse.SelectedEmployerRequests));

            return new CheckYourAnswersRespondToRequestsViewModel
            {
                Ukprn = parameters.Ukprn,
                StandardReference = parameters.StandardReference,
                StandardTitle = selectedRequests.StandardTitle,
                StandardLevel = selectedRequests.StandardLevel.ToString(),
                Website = result.Website,
                Email = providerResponse.SelectedEmail,
                HasSingleEmail = providerResponse.HasSingleEmail,
                Phone = providerResponse.SelectedPhoneNumber,
                HasSinglePhone = providerResponse.HasSinglePhoneNumber,
                SelectedRequests = selectedRequests.SelectedRequests
            };
        }

        public async Task<bool> ValidateCheckYourAnswersViewModel(CheckYourAnswersRespondToRequestsViewModel viewModel, ModelStateDictionary modelState)
        {
            return await ValidateViewModel(_employerRequestOrchestratorValidators.CheckYourAnswersViewModelValidator, viewModel, modelState);
        }

        private async Task<bool> ValidateViewModel<T>(IValidator<T> validator, T viewModel, ModelStateDictionary modelState)
        {
            await validator.ValidateAndAddModelErrorsAsync(viewModel, modelState);
            return modelState.IsValid;
        }
    }
}
