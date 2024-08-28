using FluentAssertions;
using Moq;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderWebsite;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.UnitTests.Queries.GetProviderWebsite
{
    [TestFixture]
    public class GetProviderWebsiteQueryTests
    {
        private Mock<IProviderRequestApprenticeTrainingOuterApi> _mockOuterApi;
        private GetProviderWebsiteQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockOuterApi = new Mock<IProviderRequestApprenticeTrainingOuterApi>();
            _handler = new GetProviderWebsiteQueryHandler(_mockOuterApi.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnProviderWebsite_WhenRecordsExist()
        {
            // Arrange
            var ukprn = 123456;
            var provierUrl = $"https://www.thesite.com";
            var expectedResult = new GetProviderWebsiteResponse()
            {
                Website = provierUrl
            };

            _mockOuterApi.Setup(x => x.GetProviderWebsite(ukprn))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(new GetProviderWebsiteQuery(ukprn), CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _mockOuterApi.Verify(x => x.GetProviderWebsite(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldReturnBlankEmailResponse_WhenNoRecordsExist()
        {
            // Arrange
            var ukprn = 123456;
            var expectedResult = new GetProviderWebsiteResponse()
            {
                Website = string.Empty,
            };

            _mockOuterApi.Setup(x => x.GetProviderWebsite(ukprn))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(new GetProviderWebsiteQuery(ukprn), CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _mockOuterApi.Verify(x => x.GetProviderWebsite(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public void Handle_WhenApiThrowsException_ShouldRethrowIt()
        {
            // Arrange
            _mockOuterApi.Setup(x => x.GetProviderWebsite(It.IsAny<long>()))
                .ThrowsAsync(new Exception("API failure"));

            // Act
            Func<Task> act = async () => await _handler.Handle(new GetProviderWebsiteQuery(123456), CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("API failure");
        }
    }
}
