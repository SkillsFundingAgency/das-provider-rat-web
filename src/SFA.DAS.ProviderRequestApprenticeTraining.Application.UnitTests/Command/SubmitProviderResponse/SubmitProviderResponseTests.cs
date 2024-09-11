using FluentAssertions;
using Moq;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.SubmitProviderResponse;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Requests;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.UnitTests.Commands
{
    [TestFixture]
    public class SubmitProviderResponseCommandHandlerTests
    {
        private Mock<IProviderRequestApprenticeTrainingOuterApi> _mockOuterApi;
        private SubmitProviderResponseCommandHandler _handler;
        private SubmitProviderResponseCommand _command;

        [SetUp]
        public void Setup()
        {
            _mockOuterApi = new Mock<IProviderRequestApprenticeTrainingOuterApi>();
            _handler = new SubmitProviderResponseCommandHandler(_mockOuterApi.Object);
            _command = new SubmitProviderResponseCommand
            {
                Email = "hello@email.com",
                EmployerRequestIds = new List<Guid> { Guid.Empty, Guid.Empty },
                Phone = "99944415263",
                Ukprn = 123456789,
                Website = "www.thesite.com",
                CurrentUserEmail = "user@thesite.com",
                RespondedBy = Guid.NewGuid(),
                ContactName = "Firstname Lastname",
                CurrentUserFirstName = "Firstname",
                StandardTitle = "Test Standard",
                StandardLevel = "4"
            };
        }

        [Test]
        public async Task Handle_ShouldCallOuterApiWithCorrectParameters_AndReturnProviderResponseId()
        {
            // Arrange
            var expectedResponse = new SubmitProviderResponseResponse { ProviderResponseId = Guid.NewGuid() };
            _mockOuterApi.Setup(x => x.SubmitProviderResponse(It.IsAny<long>(), It.IsAny<SubmitProviderResponseRequest>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
            _mockOuterApi.Verify(x => x.SubmitProviderResponse(It.IsAny<long>(), It.Is<SubmitProviderResponseRequest>( r => 
                r.ContactName == _command.ContactName &&
                r.CurrentUserEmail == _command.CurrentUserEmail &&
                r.CurrentUserFirstName == _command.CurrentUserFirstName &&
                r.Email == _command.Email &&
                r.EmployerRequestIds == _command.EmployerRequestIds &&
                r.Phone == _command.Phone &&
                r.RespondedBy == _command.RespondedBy &&
                r.StandardLevel == _command.StandardLevel &&
                r.StandardTitle == _command.StandardTitle &&
                r.Website == _command.Website &&
                r.EmployerRequestIds == _command.EmployerRequestIds
                )), Times.Once);
        }

        [Test]
        public void Handle_WhenApiThrowsException_ShouldRethrowIt()
        {
            // Arrange
            _mockOuterApi.Setup(x => x.SubmitProviderResponse(It.IsAny<long>(), It.IsAny<SubmitProviderResponseRequest>()))
                .ThrowsAsync(new Exception("API error"));

            // Act
            Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("API error");
        }
    }
}

