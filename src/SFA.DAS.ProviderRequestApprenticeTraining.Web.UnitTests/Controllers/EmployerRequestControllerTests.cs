using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Orchestrators;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.UnitTests.Controllers
{
    [TestFixture]
    public class EmployerRequestControllerTests
    {
        private Mock<IEmployerRequestOrchestrator> _orchestratorMock;
        private EmployerRequestController _controller;
        private Mock<IHttpContextAccessor> _httpContextMock;

        [SetUp]
        public void Setup()
        {
            _orchestratorMock = new Mock<IEmployerRequestOrchestrator>();
            _httpContextMock = new Mock<IHttpContextAccessor>();
            _controller = new EmployerRequestController(_orchestratorMock.Object, _httpContextMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test, MoqAutoData]
        public async Task AggregatedEmployerRequests_ShouldReturnViewWithViewModel(
            GetAggregatedEmployerRequestsResult aggregatedRequestResult,
            long ukprn)
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ProviderClaims.ProviderUkprn, ukprn.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);
            _httpContextMock.Setup(h => h.HttpContext.User).Returns(user);
            _orchestratorMock
                .Setup(o => o.GetActiveEmployerRequestsViewModel(ukprn))
                .ReturnsAsync(new ActiveEmployerRequestsViewModel { });

            // Act
            var result = await _controller.Active(ukprn) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<ActiveEmployerRequestsViewModel>();
        }
    }
}
