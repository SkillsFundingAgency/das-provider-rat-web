using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Configuration;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.Error;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.UnitTests.Controllers
{
    [TestFixture]
    public class ErrorControllerTests
    {
        [Test, MoqAutoData]
        public void Given_ErrorCalled_AndErrorCodeIs403_ThenViewShouldBe403(
            [Frozen] IOptions<ProviderSharedUIConfiguration> providerSharedUIConfiguration,
            [Frozen] IOptions<ProviderRequestApprenticeTrainingWebConfiguration> configuration)
        {
            // Arrange
            var sut = new ErrorController(configuration, providerSharedUIConfiguration);
            configuration.Value.RoleRequestHelpLink = "http://rolerequesthelplink.gov.uk";
            providerSharedUIConfiguration.Value.DashboardUrl = "https://dashboard.gov.uk";

            // Act
            var result = (ViewResult)sut.Error(403);

            // Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("403");
            
            var model = result.Model as Error403ViewModel;
            model.RoleRequestHelpLink.Should().BeSameAs(configuration.Value.RoleRequestHelpLink);
            model.DashboardLink.Should().BeSameAs(providerSharedUIConfiguration.Value.DashboardUrl);
        }

        [Test, MoqAutoData]
        public void Given_ErrorCalled_AndErrorCodeIs403_ThenViewShouldModelShouldContainLinks(
            [Frozen] IOptions<ProviderSharedUIConfiguration> providerSharedUIConfiguration,
            [Frozen] IOptions<ProviderRequestApprenticeTrainingWebConfiguration> configuration)
        {
            // Arrange
            var sut = new ErrorController(configuration, providerSharedUIConfiguration);
            configuration.Value.RoleRequestHelpLink = "http://rolerequesthelplink.gov.uk";
            providerSharedUIConfiguration.Value.DashboardUrl = "https://dashboard.gov.uk";

            // Act
            var result = (ViewResult)sut.Error(403);

            // Assert
            result.Should().NotBeNull();
            result.ViewName.Should().Be("403");

            var model = result.Model as Error403ViewModel;
            model.RoleRequestHelpLink.Should().BeSameAs(configuration.Value.RoleRequestHelpLink);
            model.DashboardLink.Should().BeSameAs(providerSharedUIConfiguration.Value.DashboardUrl);
        }

        [Test, MoqAutoData]
        public void Given_ErrorCalled_AndErrorCodeIs404_ThenViewShouldBe404(
            [Frozen] IOptions<ProviderSharedUIConfiguration> providerSharedUIConfiguration,
            [Frozen] IOptions<ProviderRequestApprenticeTrainingWebConfiguration> configuration)
        {
            // Arrange
            var sut = new ErrorController(configuration, providerSharedUIConfiguration);

            // Act
            var result = (ViewResult)sut.Error(404);
            
            // Assert
            result.ViewName.Should().Be("404");
        }
        
        [Test]
        [MoqInlineAutoData(401)]
        [MoqInlineAutoData(503)]
        [MoqInlineAutoData(405)]
        public void Given_ErrorCalled_AndErrorCodeIsOther_ThenViewShouldBeGeneric(int? errorCode,
            [Frozen] IOptions<ProviderSharedUIConfiguration> providerSharedUIConfiguration,
            [Frozen] IOptions<ProviderRequestApprenticeTrainingWebConfiguration> configuration)
        {
            // Arrange
            var sut = new ErrorController(configuration, providerSharedUIConfiguration);

            // Act
            var result = (ViewResult)sut.Error(errorCode);
            
            // Assert
            result.ViewName.Should().BeNull();
        }

        [Test, MoqAutoData]
        public void Given_ErrorCalled_AndErrorCodeIsNull_ThenViewShouldBeGeneric(
            [Frozen] IOptions<ProviderSharedUIConfiguration> providerSharedUIConfiguration,
            [Frozen] IOptions<ProviderRequestApprenticeTrainingWebConfiguration> configuration)
        {
            // Arrange
            var sut = new ErrorController(configuration, providerSharedUIConfiguration);

            // Act
            var result = (ViewResult)sut.Error(null);

            // Assert
            result.ViewName.Should().BeNull();
        }
    }
}
