using FluentAssertions;
using Moq;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmails;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.UnitTests.Queries.GetProviderEmails
{
    [TestFixture]
    public class GetProviderEmailsQueryTests
    {
        private Mock<IProviderRequestApprenticeTrainingOuterApi> _mockOuterApi;
        private GetProviderEmailsQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockOuterApi = new Mock<IProviderRequestApprenticeTrainingOuterApi>();
            _handler = new GetProviderEmailsQueryHandler(_mockOuterApi.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnproviderEmails_WhenRecordsExist()
        {
            // Arrange
            var ukprn = 123456;
            var expectedResult = new GetProviderEmailResponse()
            {
                EmailAddresses = new List<string> 
                {
                    "email3@provider.com",
                    "email2@provider.com",
                    "email1@provider.com"
                }
            };

            _mockOuterApi.Setup(x => x.GetProviderEmailAddresses(ukprn))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(new GetProviderEmailsQuery(ukprn), CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _mockOuterApi.Verify(x => x.GetProviderEmailAddresses(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldReturnEmptyProviderEmailresponse_WhenNoRecordsExist()
        {
            // Arrange
            var ukprn = 123456;
            var expectedResult = new GetProviderEmailResponse()
            {
                EmailAddresses = new List<string>()
            };

            _mockOuterApi.Setup(x => x.GetProviderEmailAddresses(ukprn))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(new GetProviderEmailsQuery(ukprn), CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _mockOuterApi.Verify(x => x.GetProviderEmailAddresses(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public void Handle_WhenApiThrowsException_ShouldRethrowIt()
        {
            // Arrange
            _mockOuterApi.Setup(x => x.GetProviderEmailAddresses(It.IsAny<long>()))
                .ThrowsAsync(new Exception("API failure"));

            // Act
            Func<Task> act = async () => await _handler.Handle(new GetProviderEmailsQuery(123456), CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("API failure");
        }
    }
}
