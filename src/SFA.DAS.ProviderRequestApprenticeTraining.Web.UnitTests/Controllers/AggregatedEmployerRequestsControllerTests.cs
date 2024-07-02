using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.UnitTests.Controllers
{
    [TestFixture]
    public class AggregatedEmployerRequestsControllerTests
    {
        private Mock<IMediator> _mediator;
        private Mock<IHttpContextAccessor> _contextAccessor;
        private AggregatedEmployerRequestController _controller;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();

            _controller = new AggregatedEmployerRequestController(_mediator.Object, _contextAccessor.Object);
        }
        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test, MoqAutoData]
        public async Task AggregatedEmployerRequests_ShouldReturnViewWithViewModel(
            GetAggregatedEmployerRequestsResult aggregatedRequestResult)
        {
            // Arrange
            _mediator.Setup(o => o.Send(It.IsAny<GetAggregatedEmployerRequestsQuery>(), CancellationToken.None))
                .ReturnsAsync(aggregatedRequestResult);

            // Act
            var result = await _controller.AggregatedEmployerRequests() as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<AggregatedEmployerRequestsViewModel>();
            ((AggregatedEmployerRequestsViewModel)result.Model).RequestCount.Should().Be(aggregatedRequestResult.AggregatedEmployerRequests.Count);

        }
    }
}
