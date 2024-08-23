using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequest;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.SubmitProviderResponse;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmails;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderPhoneNumbers;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderWebsite;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services.SessionStorage;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Tests.Orchestrators
{
    public class EmployerRequestOrchestratorTests
    {
        private Mock<IMediator> _mockMediator;
        private Mock<IHttpContextAccessor> _contextAccessorMock;
        private Mock<ISessionStorageService> _sessionStorageMock;
        private Mock<IOptions<ProviderSharedUIConfiguration>> _mockConfig;
        private EmployerRequestOrchestrator _sut;
        private Mock<IValidator<EmployerRequestsToContactViewModel>> _requestsToContactViewModelValidatorMock;
        private Mock<IValidator<SelectProviderEmailViewModel>> _providerEmailViewModelValidatorMock;
        private Mock<IValidator<SelectProviderPhoneViewModel>> _providerPhoneViewModelValidatorMock;
        private Mock<IValidator<CheckYourAnswersRespondToRequestsViewModel>> _checkYourAnswersViewModelValidatorMock;
        private readonly string _ukprn = "789456789";
        private readonly string _email = "hello@email.com";
        private readonly string _firstName = "Firstname";
        private readonly string _displayName = "Firstname Surname";
        private readonly string _sub = Guid.NewGuid().ToString();

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();

            _sessionStorageMock = new Mock<ISessionStorageService>();

            _requestsToContactViewModelValidatorMock = new Mock<IValidator<EmployerRequestsToContactViewModel>>();
            _providerEmailViewModelValidatorMock = new Mock<IValidator<SelectProviderEmailViewModel>>();
            _providerPhoneViewModelValidatorMock = new Mock<IValidator<SelectProviderPhoneViewModel>>();
            _checkYourAnswersViewModelValidatorMock = new Mock<IValidator<CheckYourAnswersRespondToRequestsViewModel>>();

            var employerOrchestratorValidators = new EmployerRequestOrchestratorValidators()
            {
                SelectedRequestsModelValidator = _requestsToContactViewModelValidatorMock.Object,
                SelectProviderEmailViewModelValidator = _providerEmailViewModelValidatorMock.Object,
                SelectProviderPhoneViewModelValidator = _providerPhoneViewModelValidatorMock.Object,
                CheckYourAnswersViewModelValidator = _checkYourAnswersViewModelValidatorMock.Object,
            };
            _mockConfig = new Mock<IOptions<ProviderSharedUIConfiguration>>();
            var _config = new ProviderSharedUIConfiguration
            {
                DashboardUrl = "http://example.com"
            };
            _mockConfig.Setup(o => o.Value).Returns(_config);

            _contextAccessorMock = new Mock<IHttpContextAccessor>();
            var claims = new List<Claim>
            {
                new Claim(ProviderClaims.ProviderUkprn, _ukprn),
                new Claim(ProviderClaims.Email, _email),
                new Claim(ProviderClaims.DisplayName, _displayName),
                new Claim(ProviderClaims.GivenName, _firstName),
                new Claim(ProviderClaims.Sub, _sub)
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);
            _contextAccessorMock.Setup(h => h.HttpContext.User).Returns(user);

            _sut = new EmployerRequestOrchestrator(_mockMediator.Object, 
                _sessionStorageMock.Object, employerOrchestratorValidators, _mockConfig.Object, _contextAccessorMock.Object);
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
        public async Task GetEmployerRequestsByStandardViewModel_ShouldReturnSelectEmployerRequestsViewModel(
            GetSelectEmployerRequestsResult queryResult,
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
            result.NotContactedEmployerRequests.Should().HaveCount(queryResult.SelectEmployerRequestsResponse.EmployerRequests.Where(r => !r.IsContacted).Count());
            result.ContactedEmployerRequests.Should().HaveCount(queryResult.SelectEmployerRequestsResponse.EmployerRequests.Where(r => r.IsContacted).Count());
       }

        [Test, MoqAutoData]
        public async Task GetProviderEmailViewModel_ShouldReturnGetProviderEmailViewModel(
            GetProviderEmailsResult queryResult,
            EmployerRequestsParameters param)
        {
            // Arrange

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetProviderEmailsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetProviderEmailsViewModel(param, new ModelStateDictionary());

            // Assert
            result.Should().NotBeNull();
            result.EmailAddresses.Should().BeEquivalentTo(queryResult.EmailAddresses);
        }

        [Test, MoqAutoData]
        public async Task GetProviderPhoneNumbersViewModel_ShouldReturnGetProviderPhoneNumbersViewModel(
            GetProviderPhoneNumbersResult queryResult,
            EmployerRequestsParameters param)
        {
            // Arrange

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetProviderPhoneNumbersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetProviderPhoneNumbersViewModel(param, new ModelStateDictionary());

            // Assert
            result.Should().NotBeNull();
            result.PhoneNumbers.Should().BeEquivalentTo(queryResult.PhoneNumbers);
        }

        [Test, MoqAutoData]
        public async Task GetProviderPhoneNumbersViewModel_WithSingleEmail_BackLinkShouldBeSelectRequests(
            GetProviderPhoneNumbersResult queryResult,
            EmployerRequestsParameters parameters)
        {
            // Arrange
            var providerResponse = new ProviderResponse 
            { 
                Ukprn = parameters.Ukprn ,
                HasSingleEmail = true ,
                SelectedEmail = "theSingleemail@hotmail.com",
            };
            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns(providerResponse);

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetProviderPhoneNumbersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            parameters.BackToCheckAnswers = false;

            // Act
            var result = await _sut.GetProviderPhoneNumbersViewModel(parameters, new ModelStateDictionary());

            // Assert
            result.Should().NotBeNull();
            result.BackRoute.Should().Be(EmployerRequestController.SelectRequestsToContactRouteGet);
        }

        [Test, MoqAutoData]
        public async Task GetProviderPhoneNumbersViewModel_WithMultipleEmails_BackLinkShouldBeSelectEmail(
            GetProviderPhoneNumbersResult queryResult,
            EmployerRequestsParameters parameters)
        {
            // Arrange
            var providerResponse = new ProviderResponse
            {
                Ukprn = parameters.Ukprn,
                HasSingleEmail = false,
                SelectedEmail = "onel@hotmail.com",
            };

            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns(providerResponse);

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetProviderPhoneNumbersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            parameters.BackToCheckAnswers = false;

            // Act
            var result = await _sut.GetProviderPhoneNumbersViewModel(parameters, new ModelStateDictionary());

            // Assert
            result.Should().NotBeNull();
            result.BackRoute.Should().Be(EmployerRequestController.SelectProviderEmailRouteGet);
        }

        [Test, MoqAutoData]
        public async Task GetProviderPhoneNumberViewModel_ShouldSelectSingleValue_WhenSinglePhoneNumberExists(
            EmployerRequestsParameters parameters,
            GetProviderPhoneNumbersResult queryResult,
            ProviderResponse providerResponse)
        {
            // Arrange
            queryResult.PhoneNumbers = new List<string> { "999 666 333" };

            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns(providerResponse);
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetProviderPhoneNumbersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetProviderPhoneNumbersViewModel(parameters, new ModelStateDictionary());

            // Assert
            result.Should().NotBeNull();
            result.Ukprn.Should().Be(parameters.Ukprn);
            result.SelectedPhoneNumber.Should().Be(queryResult.PhoneNumbers.First());
            result.HasSinglePhoneNumber.Should().Be(true);
            providerResponse.SelectedPhoneNumber.Should().Be(queryResult.PhoneNumbers.First());
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
                BackToCheckAnswers = true,
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
            result.BackToCheckAnswers.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task GetEmployerRequestsByStandardViewModel_ShouldReturnViewModel_WhenSessionIsEmpty(GetSelectEmployerRequestsResult queryResult)
        {
            // Arrange
            var parameters = new EmployerRequestsParameters
            {
                Ukprn = 123456,
                StandardReference = "ST00004",
                BackToCheckAnswers = false,
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
            result.BackToCheckAnswers.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task UpdateSelectedRequests_ShouldSendCreateResponseCommand_WhenNewRequestsAreViewed(long ukprn, List<Guid> selectedRequests)
        {
            // Arrange
            var viewModel = new EmployerRequestsToContactViewModel
            {
                Ukprn = ukprn,
                SelectedRequests = selectedRequests,
                ViewedEmployerRequests = new List<ViewedEmployerRequestViewModel>
                {
                    new ViewedEmployerRequestViewModel{ EmployerRequestId = Guid.NewGuid(), IsNew = true}
                }

            };
            var providerResponse = new ProviderResponse { Ukprn = ukprn };

            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns(providerResponse);

            // Act
            await _sut.UpdateSelectedRequests(viewModel);

            // Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<CreateProviderResponseEmployerRequestCommand>(), default(CancellationToken)), Times.Once);

        }

        [Test, MoqAutoData]
        public async Task UpdateSelectedRequests_ShouldNotSendCreateResponseCommand_WhenNoNewRequestsAreViewed(long ukprn, List<Guid> selectedRequests)
        {
            // Arrange
            var viewModel = new EmployerRequestsToContactViewModel
            {
                Ukprn = ukprn,
                SelectedRequests = selectedRequests,
                ViewedEmployerRequests = new List<ViewedEmployerRequestViewModel>
                {
                    new ViewedEmployerRequestViewModel{ EmployerRequestId = Guid.NewGuid(), IsNew = false}
                }

            };
            var providerResponse = new ProviderResponse { Ukprn = ukprn };

            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns(providerResponse);

            // Act
            await _sut.UpdateSelectedRequests(viewModel);

            // Assert
            _mockMediator.Verify(x => x.Send(It.IsAny<CreateProviderResponseEmployerRequestCommand>(), default(CancellationToken)), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task UpdateSelectedRequests_ShouldUpdateSelectedRequests_WhenSessionHasProviderResponse(long ukprn, List<Guid> selectedRequests)
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
            await _sut.UpdateSelectedRequests(viewModel);

            // Assert
            providerResponse.Ukprn.Should().Be(ukprn);
            providerResponse.SelectedEmployerRequests.Should().BeEquivalentTo(selectedRequests);
            _sessionStorageMock.VerifySet(s => s.ProviderResponse = providerResponse, Times.Once);
        }

        [Test, MoqAutoData]
        public async Task UpdateSelectedRequests_ShouldSetNewProviderResponse_WhenSessionIsEmpty(long ukprn, List<Guid> selectedRequests)
        {
            // Arrange
            var viewModel = new EmployerRequestsToContactViewModel
            {
                Ukprn = ukprn,
                SelectedRequests = selectedRequests
            };

            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns((ProviderResponse)null);

            // Act
            await _sut.UpdateSelectedRequests(viewModel);

            // Assert
            _sessionStorageMock.VerifySet(s => s.ProviderResponse = It.Is<ProviderResponse>(pr => pr.SelectedEmployerRequests == selectedRequests), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetSelectEmployerEmailViewModel_ShouldReturnViewModel_WhenSessionHasProviderResponse(
            EmployerRequestsParameters parameters,
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
            EmployerRequestsParameters parameters,
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
        public async Task GetSelectEmployerEmailViewModel_ShouldSelectSingleValue_WhenSingleEmailExists(
            EmployerRequestsParameters parameters,
            GetProviderEmailsResult queryResult,
            ProviderResponse providerResponse)
        {
            // Arrange
            queryResult.EmailAddresses = new List<string> { "single@email.co.uk"};

            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns(providerResponse);
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetProviderEmailsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetProviderEmailsViewModel(parameters, new ModelStateDictionary());

            // Assert
            result.Should().NotBeNull();
            result.Ukprn.Should().Be(parameters.Ukprn);
            result.SelectedEmail.Should().Be(queryResult.EmailAddresses.First());
            result.HasSingleEmail.Should().Be(true);
            providerResponse.SelectedEmail.Should().Be(queryResult.EmailAddresses.First());
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

        [Test, MoqAutoData]
        public async Task GetSelectEmployerPhoneViewModel_ShouldReturnViewModel_WhenSessionHasProviderResponse(
            EmployerRequestsParameters parameters,
            GetProviderPhoneNumbersResult queryResult,
            ProviderResponse providerResponse)
        {
            // Arrange
            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns(providerResponse);

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetProviderPhoneNumbersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetProviderPhoneNumbersViewModel(parameters, new ModelStateDictionary());

            // Assert
            result.Should().NotBeNull();
            result.Ukprn.Should().Be(parameters.Ukprn);
            result.PhoneNumbers.Should().BeEquivalentTo(queryResult.PhoneNumbers);
        }

        [Test, MoqAutoData]
        public async Task GetProviderPhoneViewModel_ShouldReturnViewModel_WhenSessionIsEmpty(
            EmployerRequestsParameters parameters,
            GetProviderPhoneNumbersResult queryResult,
            ProviderResponse providerResponse)
        {
            // Arrange
            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns((ProviderResponse)null);
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetProviderPhoneNumbersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetProviderPhoneNumbersViewModel(parameters, new ModelStateDictionary());

            // Assert
            result.Should().NotBeNull();
            result.Ukprn.Should().Be(parameters.Ukprn);
            result.PhoneNumbers.Should().BeEquivalentTo(queryResult.PhoneNumbers);
        }

        [Test, MoqAutoData]
        public void UpdateProviderPhone_ShouldUpdateProviderPhone_WhenSessionHasProviderResponse(
            SelectProviderPhoneViewModel viewModel,
            ProviderResponse providerResponse)
        {
            // Arrange

            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns(providerResponse);

            // Act
            _sut.UpdateProviderPhone(viewModel);

            // Assert
            providerResponse.SelectedPhoneNumber.Should().Be(viewModel.SelectedPhoneNumber);
            _sessionStorageMock.VerifySet(s => s.ProviderResponse = providerResponse, Times.Once);
        }

        [Test, MoqAutoData]
        public void UpdateProviderPhone_ShouldSetNewProviderResponse_WhenSessionIsEmpty(SelectProviderPhoneViewModel viewModel)
        {
            // Arrange
            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns((ProviderResponse)null);

            // Act
            _sut.UpdateProviderPhone(viewModel);

            // Assert
            _sessionStorageMock.VerifySet(s => s.ProviderResponse = It.Is<ProviderResponse>(pr => pr.SelectedPhoneNumber == viewModel.SelectedPhoneNumber), Times.Once);
        }

        [Test]
        public async Task ValidateProviderPhoneViewModel_ShouldReturnTrue_WhenModelIsValid()
        {
            // Arrange
            var viewModel = new SelectProviderPhoneViewModel();
            var modelState = new ModelStateDictionary();
            var validationResult = new ValidationResult(); // No errors

            _providerPhoneViewModelValidatorMock
                .Setup(v => v.ValidateAsync(viewModel, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _sut.ValidateProviderPhoneViewModel(viewModel, modelState);

            // Assert
            result.Should().BeTrue();
            modelState.IsValid.Should().BeTrue();
        }

        [Test]
        public async Task ValidateProviderPhoneViewModel_ShouldReturnFalse_WhenModelIsInvalid()
        {
            // Arrange
            var viewModel = new SelectProviderPhoneViewModel();
            var modelState = new ModelStateDictionary();
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("PropertyName", "Error message")
            });

            _providerPhoneViewModelValidatorMock
                .Setup(v => v.ValidateAsync(viewModel, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _sut.ValidateProviderPhoneViewModel(viewModel, modelState);

            // Assert
            result.Should().BeFalse();
            modelState.IsValid.Should().BeFalse();
            modelState["PropertyName"].Errors[0].ErrorMessage.Should().Be("Error message");
        }


        [Test]
        public void ClearProviderResponse_ShouldClearSession()
        {
            // Act
            _sut.ClearProviderResponse();

            // Assert
            _sessionStorageMock.VerifySet(s => s.ProviderResponse = null, Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetCheckYourAnswersViewModel_ShouldReturnViewModel(
            GetProviderWebsiteResult websiteResult,
            GetEmployerRequestsByIdsResult requestsResult,
            ProviderResponse providerResponse,
            EmployerRequestsParameters parameters)
        {
            // Arrange
            _mockMediator.Setup(m => m.Send(It.IsAny<GetProviderWebsiteQuery>(), default)).ReturnsAsync(websiteResult);
            _mockMediator.Setup(m => m.Send(It.IsAny<GetEmployerRequestsByIdsQuery>(), default)).ReturnsAsync(requestsResult);
            
            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns(providerResponse);

            // Act
            var result = await _sut.GetCheckYourAnswersRespondToRequestsViewModel(parameters, new ModelStateDictionary());

            // Assert
            result.Should().NotBeNull();
            result.Ukprn.Should().Be(parameters.Ukprn);
            result.StandardLevel.Should().Be(requestsResult.StandardLevel.ToString());
            result.StandardTitle.Should().Be(requestsResult.StandardTitle);
            result.StandardReference.Should().Be(requestsResult.StandardReference);
            result.Email.Should().Be(providerResponse.SelectedEmail);
            result.HasSingleEmail.Should().Be(providerResponse.HasSingleEmail);
            result.Phone.Should().Be(providerResponse.SelectedPhoneNumber);
            result.HasSinglePhone.Should().Be(providerResponse.HasSinglePhoneNumber);
            result.SelectedRequests.Count().Should().Be(requestsResult.EmployerRequests.Count);

        }

        [Test]
        public async Task ValidateCheckYourAnswersViewModel_ShouldReturnTrue_WhenModelIsValid()
        {
            // Arrange
            var viewModel = new CheckYourAnswersRespondToRequestsViewModel();
            var modelState = new ModelStateDictionary();
            var validationResult = new ValidationResult(); // No errors

            _checkYourAnswersViewModelValidatorMock
                .Setup(v => v.ValidateAsync(viewModel, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _sut.ValidateCheckYourAnswersViewModel(viewModel, modelState);

            // Assert
            result.Should().BeTrue();
            modelState.IsValid.Should().BeTrue();
        }

        [Test]
        public async Task ValidateCheckYourAnswersViewModel_ShouldReturnFalse_WhenModelIsInvalid()
        {
            // Arrange
            var viewModel = new CheckYourAnswersRespondToRequestsViewModel();
            var modelState = new ModelStateDictionary();
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("PropertyName", "Error message")
            });

            _checkYourAnswersViewModelValidatorMock
                .Setup(v => v.ValidateAsync(viewModel, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _sut.ValidateCheckYourAnswersViewModel(viewModel, modelState);

            // Assert
            result.Should().BeFalse();
            modelState.IsValid.Should().BeFalse();
            modelState["PropertyName"].Errors[0].ErrorMessage.Should().Be("Error message");
        }

        [Test, MoqAutoData]
        public async Task GetProviderResponseConfirmationViewModel_ShouldReturnViewModel(
            GetProviderResponseConfirmationResult confirmationResult,
            ProviderResponse providerResponse,
            Guid providerResponseId)
        {
            // Arrange
            _mockMediator.Setup(m => m.Send(It.IsAny<GetProviderResponseConfirmationQuery>(), default)).ReturnsAsync(confirmationResult);

            _sessionStorageMock.Setup(s => s.ProviderResponse).Returns(providerResponse);

            // Act
            var result = await _sut.GetProviderResponseConfirmationViewModel(providerResponseId);

            // Assert
            result.Should().NotBeNull();
            result.Ukprn.Should().Be(confirmationResult.Ukprn);
            result.StandardLevel.Should().Be(confirmationResult.StandardLevel.ToString());
            result.StandardTitle.Should().Be(confirmationResult.StandardTitle);
            result.Email.Should().Be(confirmationResult.Email);
            result.Phone.Should().Be(confirmationResult.Phone);
            result.SelectedRequests.Count().Should().Be(confirmationResult.EmployerRequests.Count);
        }

        [Test]
        public void GetProviderResponseConfirmationViewModel_ShouldThrowArgumentException_WhenResponseDoesNotExist()
        {
            // Arrange
            var providerResponseId = Guid.NewGuid();

            _mockMediator.Setup(m => m.Send(It.IsAny<GetProviderResponseConfirmationQuery>(), default)).ReturnsAsync((GetProviderResponseConfirmationResult)null);

            // Act
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _sut.GetProviderResponseConfirmationViewModel(providerResponseId));

            // Assert
            ex.Message.Should().Be($"The provider response {providerResponseId} was not found");
        }

        [Test, MoqAutoData]
        public async Task SubmitProviderResponse_ShouldReturnProviderResponseId_And_ClearSession_WhenResponseisCreated(
            CheckYourAnswersRespondToRequestsViewModel checkAnswersViewModel,
            SubmitProviderResponseResult providerResponseResult)
        {
            // Arrange
            _mockMediator.Setup(m => m.Send(It.IsAny<SubmitProviderResponseCommand>(), default)).ReturnsAsync(providerResponseResult);

            // Act
            var result = await _sut.SubmitProviderResponse(checkAnswersViewModel);

            // Assert
            result.Should().Be(providerResponseResult.ProviderResponseId);
            _sessionStorageMock.VerifySet(s => s.ProviderResponse = null, Times.Once);

        }


    }
}
