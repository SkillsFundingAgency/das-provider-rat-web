using FluentAssertions;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services.SessionStorage;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.UnitTests.Services.SessionStorage
{
    [TestFixture]
    public class SessionStorageServiceTests
    {
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<ISession> _sessionMock;
        private SessionStorageService _sut;

        [SetUp]
        public void Setup()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _sessionMock = new Mock<ISession>();

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Session).Returns(_sessionMock.Object);

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);

            _sut = new SessionStorageService(_httpContextAccessorMock.Object);
        }

        [Test]
        public void ProviderResponse_Get_ShouldReturnProviderResponseFromSession()
        {
            // Arrange
            var expectedProviderResponse = new ProviderResponse { Ukprn = 123456, SelectedEmployerRequests = new List<Guid> { new(), new()} };
            var serializedValue = JsonConvert.SerializeObject(expectedProviderResponse);
            var byteArray = System.Text.Encoding.UTF8.GetBytes(serializedValue);

            _sessionMock.Setup(s => s.TryGetValue(nameof(SessionStorageService.ProviderResponse), out byteArray)).Returns(true);

            // Act
            var result = _sut.ProviderResponse;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedProviderResponse);
        }

        [Test]
        public void ProviderResponse_Set_ShouldStoreProviderResponseInSession()
        {
            // Arrange
            var providerResponse = new ProviderResponse { Ukprn = 123456, SelectedEmployerRequests = new List<Guid> { new(), new() } };
            var serializedValue = JsonConvert.SerializeObject(providerResponse);
            var byteArray = System.Text.Encoding.UTF8.GetBytes(serializedValue);

            // Act
            _sut.ProviderResponse = providerResponse;

            // Assert
            _sessionMock.Verify(s => s.Set(nameof(SessionStorageService.ProviderResponse), It.Is<byte[]>(v => System.Text.Encoding.UTF8.GetString(v) == serializedValue)), Times.Once);
        }

        [Test]
        public void ProviderResponse_Set_ShouldRemoveProviderResponseFromSessionWhenValueIsNull()
        {
            // Act
            _sut.ProviderResponse = null;

            // Assert
            _sessionMock.Verify(s => s.Remove(nameof(SessionStorageService.ProviderResponse)), Times.Once);
        }
    }
}