﻿using FluentAssertions;
using Moq;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.UnitTests.Queries.GetAggregatedEmployerRequests
{
    [TestFixture]
    public class GetSelectEmployerRequestsQueryTests
    {
        private Mock<IProviderRequestApprenticeTrainingOuterApi> _mockOuterApi;
        private GetSelectEmployerRequestsQueryHandler _handler;
        private GetSelectEmployerRequestsQuery _query;

        [SetUp]
        public void Setup()
        {
            _mockOuterApi = new Mock<IProviderRequestApprenticeTrainingOuterApi>();
            _handler = new GetSelectEmployerRequestsQueryHandler(_mockOuterApi.Object);
            _query = new GetSelectEmployerRequestsQuery(12345, "query");
        }

        [Test]
        public async Task Handle_ShouldReturnSelectEmployerRequests_WhenRequestsExist()
        {
            // Arrange
            var expectedResult = new GetSelectEmployerRequestsResult()
            {
                SelectEmployerRequestsResponse = new SelectEmployerRequestsResponse
                {
                    EmployerRequests = new List<SelectEmployerRequestResponse> {
                        new SelectEmployerRequestResponse
                        {
                            DateOfRequest = "01/01/2024",
                            DeliveryMethods = new List<string> { "One", "Two", "Three" },
                            EmployerRequestId = new Guid(),
                            IsContacted = true,
                            IsNew = true,
                            Locations = new List<string> { "North", "South", "East" },
                            NumberOfApprentices = 3
                            
                        },
                        new SelectEmployerRequestResponse
                        {
                            DateOfRequest = "01/01/2024",
                            DeliveryMethods = new List<string> { "One"},
                            EmployerRequestId = new Guid(),
                            IsContacted = true,
                            IsNew = false,
                            Locations = new List<string> { "East" },
                            NumberOfApprentices = 3
                        }
                    },
                    StandardLevel = 2,
                    StandardReference = "Ref1",
                    StandardTitle="Title1",
                },
            };

            _mockOuterApi.Setup(x => x.GetSelectEmployerRequests(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResult.SelectEmployerRequestsResponse);

            // Act
            var result = await _handler.Handle(_query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _mockOuterApi.Verify(x => x.GetSelectEmployerRequests(It.IsAny<long>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldReturnEmptySelectEmployerRequests_WhenNoRequestsExist()
        {
            // Arrange
            var expectedResult = new GetSelectEmployerRequestsResult()
            {
                SelectEmployerRequestsResponse = new SelectEmployerRequestsResponse(),
            };

            _mockOuterApi.Setup(x => x.GetSelectEmployerRequests(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResult.SelectEmployerRequestsResponse);

            // Act
            var result = await _handler.Handle(_query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _mockOuterApi.Verify(x => x.GetSelectEmployerRequests(It.IsAny<long>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Handle_WhenApiThrowsException_ShouldRethrowIt()
        {
            // Arrange
            _mockOuterApi.Setup(x => x.GetSelectEmployerRequests(It.IsAny<long>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("API failure"));

            // Act
            Func<Task> act = async () => await _handler.Handle(_query, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("API failure");
        }
    }
}
