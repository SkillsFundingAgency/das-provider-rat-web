using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Tests.Orchestrators
{
    public class EmployerRequestOrchestratorTests
    {
        private Mock<IMediator> _mockMediator;
        private EmployerRequestOrchestrator _sut;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _sut = new EmployerRequestOrchestrator(_mockMediator.Object);
        }

        [Test]
        public async Task GetActiveEmployerRequestsViewModel_ShouldReturnActiveEmployerRequestsViewModel()
        {
            // Arrange
            var aggregatedRequests = new List<AggregatedEmployerRequestResponse>
            {
                new AggregatedEmployerRequestResponse { },
                new AggregatedEmployerRequestResponse { }
            };

            var queryResult = new GetAggregatedEmployerRequestsResult
            {
                AggregatedEmployerRequests = aggregatedRequests
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GetAggregatedEmployerRequestsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            // Act
            var result = await _sut.GetActiveEmployerRequestsViewModel(123456789);

            // Assert
            result.Should().NotBeNull();
            result.AggregatedEmployerRequests.Should().HaveCount(aggregatedRequests.Count);
            result.AggregatedEmployerRequests.Should().BeEquivalentTo(aggregatedRequests.Select(request => (ActiveEmployerRequestViewModel)request));
        }
    }
}
