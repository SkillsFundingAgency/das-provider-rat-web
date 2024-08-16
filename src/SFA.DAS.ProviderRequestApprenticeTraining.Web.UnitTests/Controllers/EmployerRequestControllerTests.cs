using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.UnitTests.Controllers
{
    [TestFixture]
    public class EmployerRequestControllerTests
    {
        private Mock<IEmployerRequestOrchestrator> _orchestratorMock;
        private EmployerRequestController _controller;

        [SetUp]
        public void Setup()
        {
            _orchestratorMock = new Mock<IEmployerRequestOrchestrator>();
            _controller = new EmployerRequestController(_orchestratorMock.Object);
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
    }
}
