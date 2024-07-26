using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.UpdateProviderResponseStatus;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services.SessionStorage;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Helpers;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators
{
    public class EmployerRequestOrchestrator : IEmployerRequestOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ISessionStorageService _sessionStorage;
        private readonly EmployerRequestOrchestratorValidators _employerRequestOrchestratorValidators;

        public EmployerRequestOrchestrator(IMediator mediator, ISessionStorageService sessionStorage,
            EmployerRequestOrchestratorValidators employerRequestOrchestratorValidators)
        {
            _mediator = mediator;
            _sessionStorage = sessionStorage;
            _employerRequestOrchestratorValidators = employerRequestOrchestratorValidators;
        }

        public async Task<ActiveEmployerRequestsViewModel> GetActiveEmployerRequestsViewModel(long ukprn)
        {
            var result = await _mediator.Send(new GetAggregatedEmployerRequestsQuery(ukprn));

            var model = new ActiveEmployerRequestsViewModel()
            {
                Ukprn = ukprn,
                AggregatedEmployerRequests = result.AggregatedEmployerRequests.Select(request => (ActiveEmployerRequestViewModel)request).ToList()
            };

            return model;
        }

        public async Task<SelectEmployerRequestsViewModel> GetEmployerRequestsByStandardViewModel(EmployerRequestsParameters parameters, ModelStateDictionary modelState)
        {
            var result = await _mediator.Send(new GetSelectEmployerRequestsQuery(parameters.Ukprn, parameters.StandardReference));

            var viewModel =  (SelectEmployerRequestsViewModel)result;
            viewModel.MySelectedRequests = MySessionObject.SelectedEmployerRequests;
            return viewModel;
        }

        public async Task<bool> ValidateEmployerRequestsToContactViewModel(EmployerRequestsToContactViewModel viewModel, ModelStateDictionary modelState)
        {
            return await ValidateViewModel(_employerRequestOrchestratorValidators.SelectedRequestsModelValidator, viewModel, modelState);
        }

        public async void UpdateSelectedRequests(EmployerRequestsToContactViewModel viewModel)
        {
            UpdateSessionSelectedRequests((model) =>
            {
                model.SelectedEmployerRequests = viewModel.MySelectedRequests;  
            });

            await _mediator.Send(new UpdateProviderResponseStatusCommand
            { 
                Ukprn = viewModel.Ukprn,
                EmployerRequestIds = viewModel.MySelectedRequests,
                ProviderResponseStatus = 1 
            });
        }

        private MySessionObject MySessionObject
        {
            get => _sessionStorage.MySessionObject ?? new MySessionObject();
        }

        private void UpdateSessionSelectedRequests(Action<MySessionObject> action)
        {
            var sessionObject = MySessionObject;
            action(sessionObject);
            _sessionStorage.MySessionObject = MySessionObject;
        }

        private void ClearSelectEmployerRequests()
        {
            _sessionStorage.MySessionObject = null;
        }

        private async Task<bool> ValidateViewModel<T>(IValidator<T> validator, T viewModel, ModelStateDictionary modelState)
        {
            await validator.ValidateAndAddModelErrorsAsync(viewModel, modelState);
            return modelState.IsValid;
        }
    }
}
