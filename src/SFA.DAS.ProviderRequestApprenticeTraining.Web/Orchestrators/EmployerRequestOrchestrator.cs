using MediatR;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Types;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services.SessionStorage;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.SelectEmployerRequests;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators
{
    public class EmployerRequestOrchestrator : IEmployerRequestOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ISessionStorageService _sessionStorage;

        public EmployerRequestOrchestrator(IMediator mediator, ISessionStorageService sessionStorage)
        {
            _mediator = mediator;
            _sessionStorage = sessionStorage;
        }

        public async Task<ActiveEmployerRequestsViewModel> GetActiveEmployerRequestsViewModel(long ukprn)
        {
            var result = await _mediator.Send(new GetAggregatedEmployerRequestsQuery(ukprn));

            var model = new ActiveEmployerRequestsViewModel()
            {
                AggregatedEmployerRequests = result.AggregatedEmployerRequests.Select(request => (ActiveEmployerRequestViewModel)request).ToList()
            };

            return model;
        }

        public async Task<SelectEmployerRequestsViewModel> GetSelectEmployerRequestsViewModel(long ukprn, string standardReference)
        {
            var result = await _mediator.Send(new GetSelectEmployerRequestsQuery(ukprn, standardReference));

            return (SelectEmployerRequestsViewModel)result;
        }

        public void UpdateSelectedRequests(SelectedRequestsViewModel viewModel)
        {
            UpdateSessionSelectedRequests((model) =>
            {
                model.SelectedRequestIds = viewModel.SelectedRequests;
            });
        }

        private SelectedRequestsSessionObject SessionTheBigObject
        {
            get => _sessionStorage.SelectedRequestsSessionObject ?? new SelectedRequestsSessionObject();
        }

        private void UpdateSessionSelectedRequests(Action<SelectedRequestsSessionObject> action)
        {
            var theBigObject = SessionTheBigObject;
            action(theBigObject);
            _sessionStorage.SelectedRequestsSessionObject = theBigObject;
        }

        private void ClearSessionEmployerRequest()
        {
            _sessionStorage.SelectedRequestsSessionObject = null;
        }
    }
}
