using FluentAssertions;
using Moq;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.UnitTests.Queries.GetProviderResponseConfirmation
{
    [TestFixture]
    public class GetProviderResponseConfirmationQueryTests
    {
        private Mock<IProviderRequestApprenticeTrainingOuterApi> _mockOuterApi;
        private GetProviderResponseConfirmationQueryHandler _handler;
        private GetProviderResponseConfirmationQuery _query;

        [SetUp]
        public void Setup()
        {
            _mockOuterApi = new Mock<IProviderRequestApprenticeTrainingOuterApi>();
            _handler = new GetProviderResponseConfirmationQueryHandler(_mockOuterApi.Object);
            _query = new GetProviderResponseConfirmationQuery(Guid.NewGuid());
        }

        [Test]
        public async Task Handle_ShouldReturnProviderResponseConfirmation_WhenResponseExists()
        {
            // Arrange
            var expectedResult = new GetProviderResponseConfirmationResponse()
            {

                EmployerRequests = new List<EmployerRequestResponse> {
                    new EmployerRequestResponse
                    {
                        DateOfRequest = "01/01/2024",
                        AtApprenticesWorkplace = true,
                        BlockRelease = false,
                        DayRelease = true,
                        EmployerRequestId = new Guid(),
                        Locations = new List<string> { "North", "South", "East" },
                        NumberOfApprentices = 3,
                    },
                    new EmployerRequestResponse
                    {
                        DateOfRequest = "01/01/2024",
                        AtApprenticesWorkplace = true,
                        BlockRelease = false,
                        DayRelease = true,
                        EmployerRequestId = new Guid(),
                        Locations = new List<string> { "East" },
                        NumberOfApprentices = 3,
                    }
                },
                StandardLevel = 2,
                StandardTitle = "Title1",
                Email = "email@provider.com",
                Phone = "0783654987",
                Ukprn = 10022856,
                Website = "http://www.home.com",
            };

            _mockOuterApi.Setup(x => x.GetProviderResponseConfirmation(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(_query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _mockOuterApi.Verify(x => x.GetProviderResponseConfirmation(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldReturnEmptyProviderResponseConfirmation_WhenNoRequestsExist()
        {
            // Arrange
            var expectedResult = new GetProviderResponseConfirmationResponse();

            _mockOuterApi.Setup(x => x.GetProviderResponseConfirmation(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(_query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _mockOuterApi.Verify(x => x.GetProviderResponseConfirmation(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void Handle_WhenApiThrowsException_ShouldRethrowIt()
        {
            // Arrange
            _mockOuterApi.Setup(x => x.GetProviderResponseConfirmation(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("API failure"));

            // Act
            Func<Task> act = async () => await _handler.Handle(_query, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("API failure");
        }
    }
}
