using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.UnitTests.Authorization
{
    public class ClaimUkprnAllowedAccessAuthorizationHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Given_HandleRequirementAsync_And_CanAccessService_SucceedsRequirement(
            int ukprn,
            [Frozen] Mock<IConfiguration> configuration,
            [Frozen] ProviderSharedUIConfiguration providerSharedUIConfiguration,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService)
        {
            // Arrange
            var requirement = new ClaimUkprnAllowedAccessRequirement();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString()) }));
            var context = new AuthorizationHandlerContext(new[] { requirement }, user, null);

            var configurationSectionMock = new Mock<IConfigurationSection>();
            configurationSectionMock
                .Setup(x => x.Value)
                .Returns("false");

            configuration
                .Setup(x => x.GetSection("UseStubProviderValidation"))
                .Returns(configurationSectionMock.Object);

            providerSharedUIConfiguration.DashboardUrl = "https://pas.apprenticeships.gov.uk";

            trainingProviderService.Setup(x => x.CanProviderAccessService(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var handler = new ClaimUkprnAllowedAccessAuthorizationHandler(
                trainingProviderService.Object, configuration.Object, providerSharedUIConfiguration);

            // Act
            await handler.HandleAsync(context);

            // Assert
            context.HasSucceeded.Should().Be(true);
        }

        [Test, MoqAutoData]
        public async Task Given_HandleRequirementAsync_And_CannotAccessService_SucceedsRequirement_AndRedirects(
            int ukprn,
            [Frozen] Mock<IConfiguration> configuration,
            [Frozen] ProviderSharedUIConfiguration providerSharedUIConfiguration,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService)
        {
            // Arrange
            var requirement = new ClaimUkprnAllowedAccessRequirement();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString()) }));
            
            var response = new Mock<HttpResponse>();
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Response).Returns(response.Object);

            var context = new AuthorizationHandlerContext(new[] { requirement }, user, httpContext.Object);

            var configurationSectionMock = new Mock<IConfigurationSection>();
            configurationSectionMock
                .Setup(x => x.Value)
                .Returns("false");

            configuration
                .Setup(x => x.GetSection("UseStubProviderValidation"))
                .Returns(configurationSectionMock.Object);

            providerSharedUIConfiguration.DashboardUrl = "https://pas.apprenticeships.gov.uk";

            trainingProviderService.Setup(x => x.CanProviderAccessService(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var handler = new ClaimUkprnAllowedAccessAuthorizationHandler(
                trainingProviderService.Object, configuration.Object, providerSharedUIConfiguration);

            // Act
            await handler.HandleAsync(context);

            // Assert
            response.Verify(r => r.Redirect(It.Is<string>(url => url == $"{providerSharedUIConfiguration.DashboardUrl}/error/403/invalid-status")), Times.Once);
            context.HasSucceeded.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task Given_HandleRequirementAsync_And_UkprnClaimMissing_FailsRequirement(
            int ukprn,
            [Frozen] Mock<IConfiguration> configuration,
            [Frozen] ProviderSharedUIConfiguration providerSharedUIConfiguration,
            [Frozen] Mock<ITrainingProviderService> trainingProviderService)
        {
            // Arrange
            var requirement = new ClaimUkprnAllowedAccessRequirement();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { }));
            var context = new AuthorizationHandlerContext(new[] { requirement }, user, null);

            trainingProviderService.Setup(x => x.CanProviderAccessService(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var handler = new ClaimUkprnAllowedAccessAuthorizationHandler(
                trainingProviderService.Object, configuration.Object, providerSharedUIConfiguration);

            // Act
            await handler.HandleAsync(context);

            // Assert
            context.HasSucceeded.Should().Be(false);
        }
    }
}
