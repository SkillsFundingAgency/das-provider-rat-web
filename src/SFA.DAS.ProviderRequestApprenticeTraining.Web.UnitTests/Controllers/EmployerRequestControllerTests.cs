using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
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
            ActiveEmployerRequestsViewModel viewModel)
        {
            // Arrange
            _orchestratorMock
                .Setup(o => o.GetActiveEmployerRequestsViewModel(It.IsAny<long>()))
                .ReturnsAsync(viewModel);

            // Act
            var result = await _controller.Active(123456) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<ActiveEmployerRequestsViewModel>();
        }
    }
}
