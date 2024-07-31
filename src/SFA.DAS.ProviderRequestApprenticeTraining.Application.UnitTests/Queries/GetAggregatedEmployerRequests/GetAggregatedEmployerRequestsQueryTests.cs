using FluentAssertions;
using Moq;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.UnitTests.Queries.GetAggregatedEmployerRequests
{
    [TestFixture]
    public class GetAggregatedEmployerRequestsQueryTests
    {
        private Mock<IProviderRequestApprenticeTrainingOuterApi> _mockOuterApi;
        private GetAggregatedEmployerRequestsQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockOuterApi = new Mock<IProviderRequestApprenticeTrainingOuterApi>();
            _handler = new GetAggregatedEmployerRequestsQueryHandler(_mockOuterApi.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnAggregatedEmployerRequests_WhenRequestsExist()
        {
            // Arrange
            var ukprn = 123456;
            var expectedResult = new GetAggregatedEmployerRequestsResult()
            {
                AggregatedEmployerRequests= new List<AggregatedEmployerRequestResponse>
                {
                    new AggregatedEmployerRequestResponse
                    {
                        StandardReference = "ST0032",
                        StandardTitle = "ST0108",
                        StandardSector = "Digital",
                        StandardLevel = 3,
                        NumberOfApprentices = 2,
                        NumberOfEmployers = 1,
                    }
                }
            };

            _mockOuterApi.Setup(x => x.GetAggregatedEmployerRequests(ukprn))
                .ReturnsAsync(expectedResult.AggregatedEmployerRequests);

            // Act
            var result = await _handler.Handle(new GetAggregatedEmployerRequestsQuery(ukprn), CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _mockOuterApi.Verify(x => x.GetAggregatedEmployerRequests(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldReturnEmptyAggregatedEmployerRequests_WhenNoRequestsExist()
        {
            // Arrange
            var ukprn = 123456;
            var expectedResult = new GetAggregatedEmployerRequestsResult()
            {
                AggregatedEmployerRequests = new List<AggregatedEmployerRequestResponse>()
            };

            _mockOuterApi.Setup(x => x.GetAggregatedEmployerRequests(ukprn))
                .ReturnsAsync(expectedResult.AggregatedEmployerRequests);

            // Act
            var result = await _handler.Handle(new GetAggregatedEmployerRequestsQuery(ukprn), CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _mockOuterApi.Verify(x => x.GetAggregatedEmployerRequests(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public void Handle_WhenApiThrowsException_ShouldRethrowIt()
        {
            // Arrange
            _mockOuterApi.Setup(x => x.GetAggregatedEmployerRequests(It.IsAny<long>()))
                .ThrowsAsync(new Exception("API failure"));

            // Act
            Func<Task> act = async () => await _handler.Handle(new GetAggregatedEmployerRequestsQuery(123456), CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("API failure");
        }
    }
}
