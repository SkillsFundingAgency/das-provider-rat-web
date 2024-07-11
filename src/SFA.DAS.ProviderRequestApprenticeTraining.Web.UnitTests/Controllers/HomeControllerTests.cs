using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Tests.Controllers
{
    public class HomeControllerTests
    {
        private HomeController _sut;
        private Mock<IOptions<ProviderSharedUIConfiguration>> _mockOptions;
        private ProviderSharedUIConfiguration _config;

        [SetUp]
        public void SetUp()
        {
            _config = new ProviderSharedUIConfiguration
            {
                DashboardUrl = "http://dashboard.url"
            };
            _mockOptions = new Mock<IOptions<ProviderSharedUIConfiguration>>();
            _mockOptions.Setup(x => x.Value).Returns(_config);
            _sut = new HomeController(_mockOptions.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _sut?.Dispose();
        }

        [Test]
        public void Index_ShouldRedirectToDashboardUrl()
        {
            // Act
            var result = _sut.Index() as RedirectResult;

            // Assert
            result.Should().NotBeNull();
            result.Url.Should().Be(_config.DashboardUrl);
        }

        [Test]
        public void Start_ShouldRedirectToActiveRoute()
        {
            // Act
            var result = _sut.Start() as RedirectToRouteResult;

            // Assert
            result.Should().NotBeNull();
            result.RouteName.Should().Be(EmployerRequestController.ActiveRouteGet);
        }
    }
}
