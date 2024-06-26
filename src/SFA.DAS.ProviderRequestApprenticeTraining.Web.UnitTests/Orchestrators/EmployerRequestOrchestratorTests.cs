using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Web.Orchestrators;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Types;


namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.UnitTests.Orchestrators
{
    [TestFixture]
    public class EmployerRequestOrchestratorTests
    {
        private Mock<IMediator> _mediatorMock;
        private AggregatedEmployerRequestOrchestrator _orchestrator;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();

            _orchestrator = new AggregatedEmployerRequestOrchestrator(_mediatorMock.Object);
        }

        [Test]
        public async Task GetAggregatedViewEmployerRequestsViewModel_ShouldReturnViewModel_WhenEmployerRequestsExist()
        {
            // Arrange
            var aggregatedRequests = new List<AggregatedEmployerRequest> 
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
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAggregatedEmployerRequestsQuery>(), default)).ReturnsAsync(aggregatedRequests);

            // Act
            var result = await _orchestrator.GetViewAggregatedEmployerRequestsViewModel();

            // Assert
            result.Should().NotBeNull();
            result.AggregatedEmployerRequests.Count.Should().Be(2);
        }
    }
}
