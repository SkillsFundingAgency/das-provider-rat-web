using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmails;
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
        private Mock<ISessionStorageService> _sessionStorageMock;
        private Mock<IOptions<ProviderSharedUIConfiguration>> _mockConfig;
        private EmployerRequestOrchestrator _sut;
        private Mock<IValidator<EmployerRequestsToContactViewModel>> _requestsToContactViewModelValidatorMock;
        private Mock<IValidator<SelectProviderEmailViewModel>> _providerEmailViewModelValidatorMock;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();

            _sessionStorageMock = new Mock<ISessionStorageService>();

            _requestsToContactViewModelValidatorMock = new Mock<IValidator<EmployerRequestsToContactViewModel>>();
            _providerEmailViewModelValidatorMock = new Mock<IValidator<SelectProviderEmailViewModel>>();

            var employerOrchestratorValidators = new EmployerRequestOrchestratorValidators()
            {
                SelectedRequestsModelValidator = _requestsToContactViewModelValidatorMock.Object,
                SelectProviderEmailViewModelValidator = _providerEmailViewModelValidatorMock.Object,
            };
            _mockConfig = new Mock<IOptions<ProviderSharedUIConfiguration>>();
            var _config = new ProviderSharedUIConfiguration
            {
                DashboardUrl = "http://example.com"
            };
            _mockConfig.Setup(o => o.Value).Returns(_config);

            _sut = new EmployerRequestOrchestrator(_mockMediator.Object, _sessionStorageMock.Object, employerOrchestratorValidators, _mockConfig.Object);
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

        [Test, MoqAutoData]
        public void StartProviderResponse_ShouldSetProviderResponseInSession(long ukprn)
        {
            // Act
            _sut.StartProviderResponse(ukprn);

            // Assert
            _sessionStorageMock.VerifySet(x => x.ProviderResponse = It.IsAny<ProviderResponse>(), Times.Once);
        }

        [Test]
        public async Task ValidateProviderEmailViewModel_ShouldReturnTrue_WhenModelIsValid()
        {
            // Arrange
            var viewModel = new SelectProviderEmailViewModel();
            var modelState = new ModelStateDictionary();
            var validationResult = new ValidationResult(); // No errors

            _providerEmailViewModelValidatorMock
                .Setup(v => v.ValidateAsync(viewModel, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _sut.ValidateProviderEmailsViewModel(viewModel, modelState);

            // Assert
            result.Should().BeTrue();
            modelState.IsValid.Should().BeTrue();
        }

        [Test]
        public async Task ValidateProviderEmailViewModel_ShouldReturnFalse_WhenModelIsInvalid()
        {
            // Arrange
            var viewModel = new SelectProviderEmailViewModel();
            var modelState = new ModelStateDictionary();
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("PropertyName", "Error message")
            });

            _providerEmailViewModelValidatorMock
                .Setup(v => v.ValidateAsync(viewModel, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _sut.ValidateProviderEmailsViewModel(viewModel, modelState);

            // Assert
            result.Should().BeFalse();
            modelState.IsValid.Should().BeFalse();
            modelState["PropertyName"].Errors[0].ErrorMessage.Should().Be("Error message");
        }

        [Test, MoqAutoData]
        public async Task GetEmployerRequestsByStandardViewModel_ShouldReturnViewModel_WhenSessionHasProviderResponse(GetSelectEmployerRequestsResult queryResult)
        {
            // Arrange
            var parameters = new EmployerRequestsParameters
            {
                Ukprn = 123456,
                StandardReference = "ST00004",
            };

            var providerResponse = new ProviderResponse { Ukprn = parameters.Ukprn };

            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns(providerResponse);

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetSelectEmployerRequestsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetEmployerRequestsByStandardViewModel(parameters, new ModelStateDictionary());

            // Assert
            result.Should().NotBeNull();
            result.Ukprn.Should().Be(parameters.Ukprn);
            result.SelectedRequests.Should().BeEquivalentTo(providerResponse.SelectedEmployerRequests);
        }

        [Test, MoqAutoData]
        public async Task GetEmployerRequestsByStandardViewModel_ShouldReturnViewModel_WhenSessionIsEmpty(GetSelectEmployerRequestsResult queryResult)
        {
            // Arrange
            var parameters = new EmployerRequestsParameters
            {
                Ukprn = 123456,
                StandardReference = "ST00004",
            };

            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns((ProviderResponse)null);
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetSelectEmployerRequestsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetEmployerRequestsByStandardViewModel(parameters, new ModelStateDictionary());

            // Assert
            result.Should().NotBeNull();

            result.Ukprn.Should().Be(parameters.Ukprn);
            result.SelectedRequests.Should().BeEmpty();
        }

        [Test, MoqAutoData]
        public void UpdateSelectedRequests_ShouldUpdateSelectedRequests_WhenSessionHasProviderResponse(long ukprn, List<Guid> selectedRequests)
        {
            // Arrange
            var viewModel = new EmployerRequestsToContactViewModel
            {
                Ukprn = ukprn,
                SelectedRequests = selectedRequests

            };
            var providerResponse = new ProviderResponse { Ukprn = ukprn };

            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns(providerResponse);

            // Act
            _sut.UpdateSelectedRequests(viewModel);

            // Assert
            providerResponse.Ukprn.Should().Be(ukprn);
            providerResponse.SelectedEmployerRequests.Should().BeEquivalentTo(selectedRequests);
            _sessionStorageMock.VerifySet(s => s.ProviderResponse = providerResponse, Times.Once);
        }

        [Test, MoqAutoData]
        public void UpdateSelectedRequests_ShouldSetNewProviderResponse_WhenSessionIsEmpty(long ukprn, List<Guid> selectedRequests)
        {
            // Arrange
            var viewModel = new EmployerRequestsToContactViewModel
            {
                Ukprn = ukprn,
                SelectedRequests = selectedRequests
            };

            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns((ProviderResponse)null);

            // Act
            _sut.UpdateSelectedRequests(viewModel);

            // Assert
            _sessionStorageMock.VerifySet(s => s.ProviderResponse = It.Is<ProviderResponse>(pr => pr.SelectedEmployerRequests == selectedRequests), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetSelectEmployerEmailViewModel_ShouldReturnViewModel_WhenSessionHasProviderResponse(
            GetProviderEmailsParameters parameters,
            GetProviderEmailsResult queryResult,
            ProviderResponse providerResponse)
        {
            // Arrange
            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns(providerResponse);

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetProviderEmailsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetProviderEmailsViewModel(parameters, new ModelStateDictionary());

            // Assert
            result.Should().NotBeNull();
            result.Ukprn.Should().Be(parameters.Ukprn);
            result.EmailAddresses.Should().BeEquivalentTo(queryResult.EmailAddresses);
        }

        [Test, MoqAutoData]
        public async Task GetSelectEmployerEmailViewModel_ShouldReturnViewModel_WhenSessionIsEmpty(
            GetProviderEmailsParameters parameters,
            GetProviderEmailsResult queryResult)
        {
            // Arrange
            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns((ProviderResponse)null);
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetProviderEmailsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetProviderEmailsViewModel(parameters, new ModelStateDictionary());

            // Assert
            result.Should().NotBeNull();
            result.Ukprn.Should().Be(parameters.Ukprn);
            result.EmailAddresses.Should().BeEquivalentTo(queryResult.EmailAddresses);
        }

        [Test, MoqAutoData]
        public void UpdateProviderEmail_ShouldUpdateproviderEmails_WhenSessionHasProviderResponse(
            SelectProviderEmailViewModel viewModel,
            ProviderResponse providerResponse)
        {
            // Arrange

            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns(providerResponse);

            // Act
            _sut.UpdateProviderEmail(viewModel);

            // Assert
            providerResponse.SelectedEmail.Should().Be(viewModel.SelectedEmail);
            _sessionStorageMock.VerifySet(s => s.ProviderResponse = providerResponse, Times.Once);
        }

        [Test, MoqAutoData]
        public void UpdateProviderEmail_ShouldSetNewProviderResponse_WhenSessionIsEmpty(SelectProviderEmailViewModel viewModel)
        {
            // Arrange
            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns((ProviderResponse)null);

            // Act
            _sut.UpdateProviderEmail(viewModel);

            // Assert
            _sessionStorageMock.VerifySet(s => s.ProviderResponse = It.Is<ProviderResponse>(pr => pr.SelectedEmail == viewModel.SelectedEmail), Times.Once);
        }

        [Test]
        public void ClearProviderResponse_ShouldClearSession()
        {
            // Act
            _sut.ClearProviderResponse();

            // Assert
            _sessionStorageMock.VerifySet(s => s.ProviderResponse = null, Times.Once);
        }

    }
}
