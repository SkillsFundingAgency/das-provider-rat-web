using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.UnitTests.Services
{
    public class TrainingProviderServiceTests
    {
        [Test]
        [MoqInlineAutoData(true)]
        [MoqInlineAutoData(false)]
        public async Task Given_CanProviderAccessServiceCalled_Then_ResponseReturnedForUkprn(
            bool expected,
            int ukprn,
            [Frozen] Mock<IProviderRequestApprenticeTrainingOuterApi> outerApi,
            TrainingProviderService trainingProviderService)
        {
            // Arrange
            var response = new ProviderAccountResponse
            {
                CanAccessService = expected
            };

            outerApi.Setup(p => p.GetProviderDetails(ukprn))
                .ReturnsAsync(response);

            // Act
            var actual = await trainingProviderService.CanProviderAccessService(ukprn);

            // Assert
            actual.Should().Be(response.CanAccessService);
        }
    }
}
