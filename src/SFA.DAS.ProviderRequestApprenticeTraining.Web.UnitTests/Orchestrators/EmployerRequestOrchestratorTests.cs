using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services.SessionStorage;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators;
using SFA.DAS.Testing.AutoFixture;
using ValidationResult = FluentValidation.Results.ValidationResult; 

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Tests.Orchestrators
{
    public class EmployerRequestOrchestratorTests
    {
        private Mock<IMediator> _mockMediator;
        private Mock<ISessionStorageService> _mockSessionService;
        private EmployerRequestOrchestrator _sut;
        private Mock<IValidator<EmployerRequestsToContactViewModel>> _requestsToContactViewModelValidatorMock;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _mockSessionService = new Mock<ISessionStorageService>();

            _requestsToContactViewModelValidatorMock = new Mock<IValidator<EmployerRequestsToContactViewModel>>();

            var employerOrchestratorValidators = new EmployerRequestOrchestratorValidators()
            {
                SelectedRequestsModelValidator = _requestsToContactViewModelValidatorMock.Object,
            };

            _sut = new EmployerRequestOrchestrator(_mockMediator.Object, _mockSessionService.Object, employerOrchestratorValidators);
        }

        [Test, MoqAutoData]
        public async Task GetActiveEmployerRequestsViewModel_ShouldReturnActiveEmployerRequestsViewModel(GetAggregatedEmployerRequestsResult queryResult)
        {
            // Arrange

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetAggregatedEmployerRequestsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetActiveEmployerRequestsViewModel(123456);

            // Assert
            result.Should().NotBeNull();
            result.AggregatedEmployerRequests.Should().HaveCount(queryResult.AggregatedEmployerRequests.Count);
            result.AggregatedEmployerRequests.Should().BeEquivalentTo(queryResult.AggregatedEmployerRequests.Select(request => (ActiveEmployerRequestViewModel)request));
        }

        [Test, MoqAutoData]
        public async Task GetEmployerRequestsByStandardViewModel_ShouldReturnSelectEmployerRequestsViewModel(GetSelectEmployerRequestsResult queryResult,
            EmployerRequestsParameters param)
        {
            // Arrange

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetSelectEmployerRequestsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetEmployerRequestsByStandardViewModel(param, new ModelStateDictionary());

            // Assert
            result.Should().NotBeNull();
            result.AllEmployerRequests.Should().HaveCount(queryResult.SelectEmployerRequestsResponse.EmployerRequests.Count);
            result.AllEmployerRequests.Should().BeEquivalentTo(queryResult.SelectEmployerRequestsResponse.EmployerRequests.Select(request => (SelectEmployerRequestViewModel)request));
        }

        [Test]
        public async Task ValidateEmployerRequestsToContactViewModel_ShouldReturnTrue_WhenModelIsValid()
        {
            // Arrange
            var viewModel = new EmployerRequestsToContactViewModel();
            var modelState = new ModelStateDictionary();
            var validationResult = new ValidationResult(); // No errors

            _requestsToContactViewModelValidatorMock
                .Setup(v => v.ValidateAsync(viewModel, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _sut.ValidateEmployerRequestsToContactViewModel(viewModel, modelState);

            // Assert
            result.Should().BeTrue();
            modelState.IsValid.Should().BeTrue();
        }

        [Test]
        public async Task ValidateEmployerRequestsToContactViewModel_ShouldReturnFalse_WhenModelIsInvalid()
        {
            // Arrange
            var viewModel = new EmployerRequestsToContactViewModel();
            var modelState = new ModelStateDictionary();
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("PropertyName", "Error message")
            });

            _requestsToContactViewModelValidatorMock
                .Setup(v => v.ValidateAsync(viewModel, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _sut.ValidateEmployerRequestsToContactViewModel(viewModel, modelState);

            // Assert
            result.Should().BeFalse();
            modelState.IsValid.Should().BeFalse();
            modelState["PropertyName"].Errors[0].ErrorMessage.Should().Be("Error message");
        }
    }
}
