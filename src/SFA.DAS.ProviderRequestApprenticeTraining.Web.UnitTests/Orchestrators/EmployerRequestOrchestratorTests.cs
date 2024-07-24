using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Tests.Orchestrators
{
    public class EmployerRequestOrchestratorTests
    {
        private Mock<IMediator> _mockMediator;
        private Mock<IOptions<ProviderSharedUIConfiguration>> _mockConfig;
        private EmployerRequestOrchestrator _sut;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _mockConfig = new Mock<IOptions<ProviderSharedUIConfiguration>>();
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

            var _config = new ProviderSharedUIConfiguration
            {
                DashboardUrl = "http://example.com"
            };
            _mockConfig.Setup(o => o.Value).Returns(_config);

            _sut = new EmployerRequestOrchestrator(_mockMediator.Object, _mockConfig.Object);

            // Act
            var result = await _sut.GetActiveEmployerRequestsViewModel(123456789);

            // Assert
            result.Should().NotBeNull();
            result.AggregatedEmployerRequests.Should().HaveCount(aggregatedRequests.Count);
            result.AggregatedEmployerRequests.Should().BeEquivalentTo(aggregatedRequests.Select(request => (ActiveEmployerRequestViewModel)request));
            result.BackLink.Should().Be(_config.DashboardUrl);
        }
    }
}
