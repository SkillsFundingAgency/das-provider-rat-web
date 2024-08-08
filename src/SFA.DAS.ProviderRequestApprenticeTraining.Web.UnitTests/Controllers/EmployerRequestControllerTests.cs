using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
        private Mock<IOptions<ProviderUrlConfiguration>> _providerUrlConfiguration;
        private Mock<IHttpContextAccessor> _contextAccessorMock;
        private EmployerRequestController _controller;
        private readonly string _ukprn = "789456789";
        private readonly string _email = "hello@email.com";

        [SetUp]
        public void Setup()
        {
            _orchestratorMock = new Mock<IEmployerRequestOrchestrator>();
            _providerUrlConfiguration = new Mock<IOptions<ProviderUrlConfiguration>>();

            _contextAccessorMock = new Mock<IHttpContextAccessor>();
            var claims = new List<Claim>
            {
                new Claim(ProviderClaims.ProviderUkprn, _ukprn),
                new Claim(ProviderClaims.Email, _email)
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);
            _contextAccessorMock.Setup(h => h.HttpContext.User).Returns(user);

            _controller = new EmployerRequestController(
                _orchestratorMock.Object, 
                _providerUrlConfiguration.Object, 
                _contextAccessorMock.Object);
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
            GetProviderEmailsParameters parameters)
        {
            // Arrange
            viewModel.EmailAddresses = new List<string> { "first@hotmail.com", "second@hotmail.com" };
            _orchestratorMock
                .Setup(o => o.GetProviderEmailsViewModel(It.IsAny<GetProviderEmailsParameters>(), It.IsAny<ModelStateDictionary>()))
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
            GetProviderEmailsParameters parameters)
        {
            // Arrange
            viewModel.EmailAddresses = new List<string> { "onlyone@hotmail.com" };

            _orchestratorMock
                .Setup(o => o.GetProviderEmailsViewModel(It.IsAny<GetProviderEmailsParameters>(), It.IsAny<ModelStateDictionary>()))
                .ReturnsAsync(viewModel);

            // Act
            var result = await _controller.SelectProviderEmail(parameters) as RedirectToRouteResult;

            // Assert
            result.Should().NotBeNull();

            //Temporarily directing to SelectRequest.  Will be Phone or Check Answers when full path is implemented
            result.RouteName.Should().Be(EmployerRequestController.SelectRequestsToContactRouteGet);
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
                EmailAddresses = new List<string> { "one@hotmail.com", "two@hotmail.com" }
            };

            _orchestratorMock.Setup(o => o.ValidateProviderEmailsViewModel(viewModel, It.IsAny<ModelStateDictionary>())).ReturnsAsync(true);

            // Act
            await _controller.SelectProviderEmail(viewModel);

            // Assert
            _orchestratorMock.Verify(o => o.UpdateProviderEmail(viewModel), Times.Once);
        }

        [Test, MoqAutoData]
        public void RedirectToManageStandardsGet_ShouldRedirectToManageStandards(
            [Frozen] Mock<IOptions<ProviderUrlConfiguration>> mockConfig,
            long ukprn)
        {
            //Arrange
            var controller = new EmployerRequestController(_orchestratorMock.Object, mockConfig.Object, _contextAccessorMock.Object);
            var expectedUrl = $"{mockConfig.Object.Value.CourseManagementBaseUrl}/{ukprn}/review-your-details";

            // Act
            var result = controller.RedirectToManageStandards(ukprn) as RedirectResult;

            // Assert
            result.Should().NotBeNull();
            result.Url.Should().Be(expectedUrl);

            _orchestratorMock.Verify(o => o.ClearProviderResponse(), Times.Once);
        }
    }
}
