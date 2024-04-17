using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.UnitTests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test, MoqAutoData]
        public void Given_IndexCalled_ThenRedirectToDashboardUrl(
            [Frozen] IOptions<ProviderSharedUIConfiguration> providerSharedUIConfiguration)
        {
            // Arrange
            var sut = new HomeController(providerSharedUIConfiguration);
            providerSharedUIConfiguration.Value.DashboardUrl = "https://dashboard.gov.uk";

            // Act
            var result = (RedirectResult)sut.Index();

            // Assert
            result.Should().NotBeNull();
            result.Url.Should().Be(providerSharedUIConfiguration.Value.DashboardUrl);
        }

        [Test, MoqAutoData]
        public void Given_StartCalled_ThenViewShouldBeReturned(
            [Frozen] IOptions<ProviderSharedUIConfiguration> providerSharedUIConfiguration)
        {
            // Arrange
            var sut = new HomeController(providerSharedUIConfiguration);
            providerSharedUIConfiguration.Value.DashboardUrl = "https://dashboard.gov.uk";

            // Act
            var result = (ViewResult)sut.Start();

            // Assert
            result.Should().NotBeNull();
        }
    }
}
