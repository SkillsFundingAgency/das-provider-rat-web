using FluentAssertions;
using Moq;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderPhoneNumbers;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.UnitTests.Queries.GetProviderPhoneNumbers
{
    [TestFixture]
    public class GetProviderPhoneNumbersQueryTests
    {
        private Mock<IProviderRequestApprenticeTrainingOuterApi> _mockOuterApi;
        private GetProviderPhoneNumbersQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockOuterApi = new Mock<IProviderRequestApprenticeTrainingOuterApi>();
            _handler = new GetProviderPhoneNumbersQueryHandler(_mockOuterApi.Object);
        }

        [Test]
        public async Task Handle_ShouldReturn_ProviderPhoneNumbers_WhenRecordsExist()
        {
            // Arrange
            var ukprn = 123456;
            var expectedResult = new GetProviderPhoneNumbersResponse()
            {
                PhoneNumbers = new List<string> 
                {
                    "0784 123456",
                    "1478521455445",
                    "01245 123456789"
                }
            };

            _mockOuterApi.Setup(x => x.GetProviderPhoneNumbers(ukprn))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(new GetProviderPhoneNumbersQuery(ukprn), CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _mockOuterApi.Verify(x => x.GetProviderPhoneNumbers(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldReturnEmpty_ProviderPhoneNumbersResponse_WhenNoRecordsExist()
        {
            // Arrange
            var ukprn = 123456;
            var expectedResult = new GetProviderPhoneNumbersResponse()
            {
                PhoneNumbers = new List<string>()
            };

            _mockOuterApi.Setup(x => x.GetProviderPhoneNumbers(ukprn))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(new GetProviderPhoneNumbersQuery(ukprn), CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _mockOuterApi.Verify(x => x.GetProviderPhoneNumbers(It.IsAny<long>()), Times.Once);
        }

        [Test]
        public void Handle_WhenApiThrowsException_ShouldRethrowIt()
        {
            // Arrange
            _mockOuterApi.Setup(x => x.GetProviderPhoneNumbers(It.IsAny<long>()))
                .ThrowsAsync(new Exception("API failure"));

            // Act
            Func<Task> act = async () => await _handler.Handle(new GetProviderPhoneNumbersQuery(123456), CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("API failure");
        }
    }
}
