using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using System.Security.Claims;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        private HomeController _sut;
        private Mock<IOptions<ProviderSharedUIConfiguration>> _mockOptions;
        private ProviderSharedUIConfiguration _config;
        private Mock<IHttpContextAccessor> _httpContextMock;
        private Mock<ILogger<HomeController>> _loggerMock;
        private readonly string _ukprn = "789456789";

        [SetUp]
        public void SetUp()
        {
            _config = new ProviderSharedUIConfiguration
            {
                DashboardUrl = "http://dashboard.url/"
            };
            _loggerMock = new Mock<ILogger<HomeController>>();
            _mockOptions = new Mock<IOptions<ProviderSharedUIConfiguration>>();
            _mockOptions.Setup(x => x.Value).Returns(_config);

            _httpContextMock = new Mock<IHttpContextAccessor>();
            var claims = new List<Claim>
            {
                new Claim(ProviderClaims.ProviderUkprn, _ukprn)
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);
            _httpContextMock.Setup(h => h.HttpContext.User).Returns(user);

            _sut = new HomeController(_mockOptions.Object, _httpContextMock.Object, _loggerMock.Object);
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
            result.Url.Should().Be(_config.DashboardUrl + "account");
        }
    }
}
