using FluentAssertions;
using Moq;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.UnitTests.Queries.GetEmployerRequestsByIds
{
    [TestFixture]
    public class GetCheckYourAnswersQueryTests
    {
        private Mock<IProviderRequestApprenticeTrainingOuterApi> _mockOuterApi;
        private GetCheckYourAnswersQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockOuterApi = new Mock<IProviderRequestApprenticeTrainingOuterApi>();
            _handler = new GetCheckYourAnswersQueryHandler(_mockOuterApi.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnRequestsWithGivenIds_WhenRequestsExist()
        {
            // Arrange
            var ukprn = 123456;
            var requestids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var expectedResult = new GetCheckYourAnswersResponse()
            {
                StandardTitle = "Title 1",
                StandardLevel = 1,
                StandardReference = "Ref1",
                Website ="www.theweb.com",
                Requests = new List<EmployerRequestResponse>
                {
                    new EmployerRequestResponse
                    {
                        DateOfRequest =  DateTime.Now.AddDays(-5).ToString("dd/M/yyyy"),
                        AtApprenticesWorkplace = true,
                        BlockRelease = true,
                        DayRelease = true,
                        EmployerRequestId = requestids[0],
                        Locations = new List<string> {"North","South", "East"},
                        NumberOfApprentices = 2
                    },
                    new EmployerRequestResponse
                    {
                        DateOfRequest =  DateTime.Now.AddDays(-3).ToString("dd/M/yyyy"),
                        AtApprenticesWorkplace = true,
                        BlockRelease = false,
                        DayRelease = false,
                        EmployerRequestId = requestids[1],
                        Locations = new List<string> {"North"},
                        NumberOfApprentices = 4
                    }
                }
            };

            _mockOuterApi.Setup(x => x.GetCheckYourAnswers(ukprn, requestids)).ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(new GetCheckYourAnswersQuery(ukprn, requestids), CancellationToken.None);

            // Assert
            result.EmployerRequests.Should().BeEquivalentTo(expectedResult.Requests);
            _mockOuterApi.Verify(x => x.GetCheckYourAnswers(ukprn, requestids), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldReturnEmptyEmployerRequests_WhenNoRequestsExist()
        {
            // Arrange
            var expectedResult = new GetCheckYourAnswersResponse()
            {
                Requests = new List<EmployerRequestResponse>(),
                Website = "",
            };

            _mockOuterApi.Setup(x => x.GetCheckYourAnswers(It.IsAny<long>(), It.IsAny<List<Guid>>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(new GetCheckYourAnswersQuery(123456, new List<Guid>()), CancellationToken.None);

            // Assert
            result.EmployerRequests.Should().BeEquivalentTo(expectedResult.Requests);
            _mockOuterApi.Verify(x => x.GetCheckYourAnswers(It.IsAny<long>(), It.IsAny<List<Guid>>()), Times.Once);
        }

        [Test]
        public void Handle_WhenApiThrowsException_ShouldRethrowIt()
        {
            // Arrange
            _mockOuterApi.Setup(x => x.GetCheckYourAnswers(It.IsAny<long>(), It.IsAny<List<Guid>>()))
                .ThrowsAsync(new Exception("API failure"));

            // Act
            Func<Task> act = async () => await _handler.Handle(new GetCheckYourAnswersQuery(123456, new List<Guid>()), CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("API failure");
        }
    }
}
