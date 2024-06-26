using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Web.Orchestrators;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Types;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.UnitTests.Controllers
{
    [TestFixture]
    public class AggregatedEmployerRequestsControllerTests
    {
        private Mock<IAggregatedEmployerRequestOrchestrator> _orchestratorMock;
        private AggregatedEmployerRequestController _controller;

        [SetUp]
        public void Setup()
        {
            _orchestratorMock = new Mock<IAggregatedEmployerRequestOrchestrator>();

            _controller = new AggregatedEmployerRequestController(_orchestratorMock.Object);
        }
        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test]
        public async Task AggregatedEmployerRequests_ShouldReturnViewWithViewModel()
        {
            // Arrange
            var viewModel = new GetViewAggregatedEmployerRequestsViewModel
            {
                AggregatedEmployerRequests = new List<AggregatedEmployerRequest> 
                { 
                    new AggregatedEmployerRequest
                    { 
                        StandardReference = "ST0001",
                        StandardTitle = "Actuarial technician",
                        StandardSector = "Business and administration",
                        StandardLevel = 1,
                        NumberOfApprentices = 3,
                        NumberOfEmployers = 2,
                    },
                    new AggregatedEmployerRequest
                    {
                        StandardReference = "ST0002",
                        StandardTitle = "Aerospace engineer",
                        StandardSector = "Engineering",
                        StandardLevel = 3,
                        NumberOfApprentices = 5,
                        NumberOfEmployers = 1,
                    },
                }
            };

            _orchestratorMock.Setup(o => o.GetViewAggregatedEmployerRequestsViewModel()).ReturnsAsync(viewModel);

            // Act
            var result = await _controller.AggregatedEmployerRequests() as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().BeEquivalentTo(viewModel);
        }
    }
}
