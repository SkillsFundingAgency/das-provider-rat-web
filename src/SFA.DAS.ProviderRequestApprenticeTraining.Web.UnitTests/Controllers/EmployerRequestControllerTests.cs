﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Configuration;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.UnitTests.Controllers
{
    [TestFixture]
    public class EmployerRequestControllerTests
    {
        private Mock<IEmployerRequestOrchestrator> _orchestratorMock;
        private Mock<IOptions<ProviderRequestApprenticeTrainingWebConfiguration>> _webConfiguration;
        private EmployerRequestController _controller;


        [SetUp]
        public void Setup()
        {
            _orchestratorMock = new Mock<IEmployerRequestOrchestrator>();
            _webConfiguration = new Mock<IOptions<ProviderRequestApprenticeTrainingWebConfiguration>>();

            _controller = new EmployerRequestController(
                _orchestratorMock.Object, 
                _webConfiguration.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test, MoqAutoData]
        public async Task AggregatedEmployerRequests_ShouldReturnViewWithViewModel(
            ActiveEmployerRequestsViewModel viewModel,
            long ukprn)
        {
            // Arrange
            _orchestratorMock
                .Setup(o => o.GetActiveEmployerRequestsViewModel(ukprn))
                .ReturnsAsync(viewModel);

            // Act
            var result = await _controller.Active(ukprn) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<ActiveEmployerRequestsViewModel>();
        }

        [Test, MoqAutoData]
        public async Task SelectRequestsToContactGet_ShouldReturnViewWithViewModel(
            SelectEmployerRequestsViewModel viewModel,
            EmployerRequestsParameters parameters)
        {
            // Arrange
            _orchestratorMock
                .Setup(o => o.GetEmployerRequestsByStandardViewModel(parameters, It.IsAny<ModelStateDictionary>()))
                .ReturnsAsync(viewModel);

            // Act
            var result = await _controller.SelectRequestsToContact(parameters) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<SelectEmployerRequestsViewModel>();
        }

        [Test]
        public async Task SelectRequestsToContactPost_ShouldRedirectToSelectRequestsToContactWhenModelStateIsInvalid()
        {
            // Arrange
            var viewModel = new EmployerRequestsToContactViewModel
            {
                Ukprn = 789456,
                StandardReference = "ST0004",
                SelectedRequests = new List<Guid> { new(), new()}
            };

            _orchestratorMock.Setup(o => o.ValidateEmployerRequestsToContactViewModel(viewModel, It.IsAny<ModelStateDictionary>())).ReturnsAsync(false);

            // Act
            var result = await _controller.SelectRequestsToContact(viewModel) as RedirectToRouteResult;

            // Assert
            result.Should().NotBeNull();
            result.RouteName.Should().Be(EmployerRequestController.SelectRequestsToContactRouteGet);
            result.RouteValues["ukprn"].Should().Be(viewModel.Ukprn);
            result.RouteValues["standardReference"].Should().Be(viewModel.StandardReference);
        }

        [Test]
        public async Task SelectRequestsToContactPost_ShouldCallUpdateSelectedRequestsWhenModelStateIsValid()
        {
            // Arrange
            var viewModel = new EmployerRequestsToContactViewModel
            {
                Ukprn = 789456,
                StandardReference = "ST0004",
                SelectedRequests = new List<Guid> { new(), new() }
            };

            _orchestratorMock.Setup(o => o.ValidateEmployerRequestsToContactViewModel(viewModel, It.IsAny<ModelStateDictionary>())).ReturnsAsync(true);

            // Act
            await _controller.SelectRequestsToContact(viewModel);

            // Assert
            _orchestratorMock.Verify(o => o.UpdateSelectedRequests(viewModel), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task SelectProviderEmailGet_ShouldReturnViewWithViewModel(
            SelectProviderEmailViewModel viewModel,
            EmployerRequestsParameters parameters)
        {
            // Arrange
            viewModel.HasSingleEmail = false;
            viewModel.EmailAddresses = new List<string> { "first@hotmail.com", "second@hotmail.com" };
            _orchestratorMock
                .Setup(o => o.GetProviderEmailsViewModel(It.IsAny<EmployerRequestsParameters>(), It.IsAny<ModelStateDictionary>()))
                .ReturnsAsync(viewModel);

            // Act
            var result = await _controller.SelectProviderEmail(parameters) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<SelectProviderEmailViewModel>();
            result.Model.Should().BeEquivalentTo(viewModel);
        }

        [Test, MoqAutoData]
        public async Task SelectProviderEmailGet_ShouldRedirectToPhone_WhenSingleEmail(
            SelectProviderEmailViewModel viewModel,
            EmployerRequestsParameters parameters)
        {
            // Arrange
            viewModel.HasSingleEmail = true;
            viewModel.EmailAddresses = new List<string> { "onlyone@hotmail.com" };

            _orchestratorMock
                .Setup(o => o.GetProviderEmailsViewModel(It.IsAny<EmployerRequestsParameters>(), It.IsAny<ModelStateDictionary>()))
                .ReturnsAsync(viewModel);

            // Act
            var result = await _controller.SelectProviderEmail(parameters) as RedirectToRouteResult;

            // Assert
            result.Should().NotBeNull();
  
            result.RouteName.Should().Be(EmployerRequestController.SelectProviderPhoneRouteGet);
        }

        [Test]
        public async Task SelectProviderEmailPost_ShouldRedirectToSelectProviderEmailGetWhenModelStateIsInvalid()
        {
            // Arrange
            var viewModel = new SelectProviderEmailViewModel
            {
                Ukprn = 789456,
                EmailAddresses = new List<string> { "one@hotmail.com", "two@hotmail.com"}
            };

            _orchestratorMock.Setup(o => o.ValidateProviderEmailsViewModel(viewModel, It.IsAny<ModelStateDictionary>())).ReturnsAsync(false);

            // Act
            var result = await _controller.SelectProviderEmail(viewModel) as RedirectToRouteResult;

            // Assert
            result.Should().NotBeNull();
            result.RouteName.Should().Be(EmployerRequestController.SelectProviderEmailRouteGet);
            result.RouteValues["ukprn"].Should().Be(viewModel.Ukprn);
        }

        [Test]
        public async Task SelectProviderEmailPost_ShouldCallUpdateProviderEmailsWhenModelStateIsValid()
        {
            // Arrange
            var viewModel = new SelectProviderEmailViewModel
            {
                Ukprn = 789456,
                EmailAddresses = new List<string> { "one@hotmail.com", "two@hotmail.com" },
                BackToCheckAnswers = false
            };

            _orchestratorMock.Setup(o => o.ValidateProviderEmailsViewModel(viewModel, It.IsAny<ModelStateDictionary>())).ReturnsAsync(true);

            // Act
            await _controller.SelectProviderEmail(viewModel);

            // Assert
            _orchestratorMock.Verify(o => o.UpdateProviderEmail(viewModel), Times.Once);
        }

        [Test]
        public async Task SelectProviderEmailPost_ShouldRedirectToCheckYourAnswersGetWhenModelStateIsValidAndBackToCheckAnswersIsTrue()
        {
            // Arrange
            var viewModel = new SelectProviderEmailViewModel
            {
                Ukprn = 789456,
                EmailAddresses = new List<string> { "one@hotmail.com", "two@hotmail.com" },
                BackToCheckAnswers = true,
            };

            _orchestratorMock.Setup(o => o.ValidateProviderEmailsViewModel(viewModel, It.IsAny<ModelStateDictionary>())).ReturnsAsync(true);

            // Act
            var result = await _controller.SelectProviderEmail(viewModel) as RedirectToRouteResult;

            // Assert
            result.Should().NotBeNull();
            result.RouteName.Should().Be(EmployerRequestController.CheckYourAnswersRouteGet);
            result.RouteValues["ukprn"].Should().Be(viewModel.Ukprn);
        }

        [Test]
        public async Task SelectProviderEmailPost_ShouldRedirectToSelectPhoneGetWhenModelStateIsValidAndBackToCheckAnswersIsFalse()
        {
            // Arrange
            var viewModel = new SelectProviderEmailViewModel
            {
                Ukprn = 789456,
                EmailAddresses = new List<string> { "one@hotmail.com", "two@hotmail.com" },
                BackToCheckAnswers = false,
            };

            _orchestratorMock.Setup(o => o.ValidateProviderEmailsViewModel(viewModel, It.IsAny<ModelStateDictionary>())).ReturnsAsync(true);

            // Act
            var result = await _controller.SelectProviderEmail(viewModel) as RedirectToRouteResult;

            // Assert
            result.Should().NotBeNull();
            result.RouteName.Should().Be(EmployerRequestController.SelectProviderPhoneRouteGet);
            result.RouteValues["ukprn"].Should().Be(viewModel.Ukprn);
        }


        [Test, MoqAutoData]
        public void RedirectToManageStandardsGet_ShouldRedirectToManageStandards(
            [Frozen] Mock<IOptions<ProviderRequestApprenticeTrainingWebConfiguration>> mockConfig,
            long ukprn)
        {
            //Arrange
            var controller = new EmployerRequestController(_orchestratorMock.Object, mockConfig.Object);
            var expectedUrl = $"{mockConfig.Object.Value.CourseManagementBaseUrl}{ukprn}/review-your-details";

            // Act
            var result = controller.RedirectToManageStandards(ukprn) as RedirectResult;

            // Assert
            result.Should().NotBeNull();
            result.Url.Should().Be(expectedUrl);

            _orchestratorMock.Verify(o => o.ClearProviderResponse(), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task SelectProviderPhoneNumbersGet_MoreThanOnePhoneNumber_ShouldReturnViewWithViewModel(
                SelectProviderPhoneViewModel viewModel,
                EmployerRequestsParameters parameters)
        {
            // Arrange
            viewModel.PhoneNumbers = new List<string> { "one", "two", "three" };
            viewModel.HasSinglePhoneNumber = false;

            _orchestratorMock
                .Setup(o => o.GetProviderPhoneNumbersViewModel(parameters, It.IsAny<ModelStateDictionary>()))
                .ReturnsAsync(viewModel);

            // Act
            var result = await _controller.SelectProviderPhoneNumber(parameters) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<SelectProviderPhoneViewModel>();
        }

        [Test, MoqAutoData]
        public async Task SelectProviderPhoneGet_OnePhoneNumber_ShouldRedirectToCheckYourAnswers(
            SelectProviderPhoneViewModel phoneViewModel,
            EmployerRequestsParameters parameters)
        {
            // Arrange
            phoneViewModel.PhoneNumbers = new List<string> { "123456789" };
            phoneViewModel.HasSinglePhoneNumber = true;

            _orchestratorMock
                .Setup(o => o.GetProviderPhoneNumbersViewModel(It.IsAny<EmployerRequestsParameters>(), It.IsAny<ModelStateDictionary>()))
                .ReturnsAsync(phoneViewModel);

            // Act
            var result = await _controller.SelectProviderPhoneNumber(parameters) as RedirectToRouteResult;

            // Assert
            result.Should().NotBeNull();

            result.RouteName.Should().Be(EmployerRequestController.CheckYourAnswersRouteGet);
        }

        [Test]
        public async Task SelectProviderPhoneNumbersPost_ShouldCallUpdateProviderPhone_WhenModelStateIsValid()
        {
            // Arrange
            var viewModel = new SelectProviderPhoneViewModel
            {
                Ukprn = 789456,
                PhoneNumbers = new List<string> { "07834660123", "t0784 789456" }
            };

            _orchestratorMock.Setup(o => o.ValidateProviderPhoneViewModel(viewModel, It.IsAny<ModelStateDictionary>())).ReturnsAsync(true);

            // Act
            await _controller.SelectProviderPhoneNumber(viewModel);

            // Assert
            _orchestratorMock.Verify(o => o.UpdateProviderPhone(viewModel), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task ChecKYourAnswersGet_ShouldReturnViewWithViewModel(
            CheckYourAnswersRespondToRequestsViewModel viewModel,
            EmployerRequestsParameters parameters)
        {
            // Arrange
            _orchestratorMock
                .Setup(o => o.GetCheckYourAnswersRespondToRequestsViewModel(It.IsAny<EmployerRequestsParameters>(), It.IsAny<ModelStateDictionary>()))
                .ReturnsAsync(viewModel);

            _orchestratorMock.Setup(o => o.ValidateCheckYourAnswersViewModel(viewModel, It.IsAny<ModelStateDictionary>())).ReturnsAsync(true);

            // Act
            var result = await _controller.CheckYourAnswers(parameters) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<CheckYourAnswersRespondToRequestsViewModel>();
            result.Model.Should().BeEquivalentTo(viewModel);
        }

        [Test, MoqAutoData]
        public async Task ChecKYourAnswersGet_ShouldRedirectToActiveRequests_WhenModelIsInvalid(
            CheckYourAnswersRespondToRequestsViewModel viewModel,
            EmployerRequestsParameters parameters)
        {
            // Arrange
            _orchestratorMock
                .Setup(o => o.GetCheckYourAnswersRespondToRequestsViewModel(It.IsAny<EmployerRequestsParameters>(), It.IsAny<ModelStateDictionary>()))
                .ReturnsAsync(viewModel);

            _orchestratorMock.Setup(o => o.ValidateCheckYourAnswersViewModel(viewModel, It.IsAny<ModelStateDictionary>())).ReturnsAsync(false);

            // Act
            var result = await _controller.CheckYourAnswers(parameters) as RedirectToRouteResult;

            // Assert
            result.Should().NotBeNull();
            result.RouteName.Should().Be(EmployerRequestController.ActiveRouteGet);
            result.RouteValues["ukprn"].Should().Be(viewModel.Ukprn);
            _orchestratorMock.Verify(x => x.ClearProviderResponse(), Times.Once);
        }

        [Test]
        public async Task CheckYourAnswersPost_ShouldRedirectToConfirmation_WhenIsValid()
        {
            // Arrange
            var viewModel = new CheckYourAnswersRespondToRequestsViewModel
            {
                Ukprn = 789456,
                StandardReference = "ST0004"
            };

            _orchestratorMock.Setup(o => o.ValidateCheckYourAnswersViewModel(viewModel, It.IsAny<ModelStateDictionary>())).ReturnsAsync(true);

            // Act
            var result = await _controller.CheckYourAnswers(viewModel) as RedirectToRouteResult;

            // Assert
            result.Should().NotBeNull();
            result.RouteName.Should().Be(EmployerRequestController.ConfirmationRouteGet);
            result.RouteValues["providerResponseId"].Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public void CancelGet_ShouldRedirectToActiveRequests(long ukprn)
        {
            // Act
            var result = _controller.Cancel(ukprn) as RedirectToRouteResult;

            // Assert
            result.Should().NotBeNull();
            result.RouteName.Should().Be(EmployerRequestController.ActiveRouteGet);
            result.RouteValues["ukprn"].Should().Be(ukprn);
            _orchestratorMock.Verify(x => x.ClearProviderResponse(), Times.Once);
        }
    }
}
