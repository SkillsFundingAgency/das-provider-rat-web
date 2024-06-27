using MediatR;
using Moq;
using NUnit.Framework;



namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.UnitTests.Orchestrators
{
    [TestFixture]
    public class EmployerRequestOrchestratorTests
    {
        private Mock<IMediator> _mediatorMock;
        private IMediator _mediator;

        [SetUp]
        public void Setup()
        {
        //    _mediatorMock = new Mock<IMediator>();

        //    _mediator = new AggregatedEmployerRequestOrchestrator(_mediatorMock.Object);
        //
        }

        [Test]
        public async Task GetAggregatedViewEmployerRequestsViewModel_ShouldReturnViewModel_WhenEmployerRequestsExist()
        {
            //// Arrange
            //var aggregatedRequests = new List<AggregatedEmployerRequest> 
            //{
            //    new AggregatedEmployerRequest
            //    {
            //        StandardReference = "ST0001",
            //        StandardTitle = "Actuarial technician",
            //        StandardSector = "Business and administration",
            //        StandardLevel = 1,
            //        NumberOfApprentices = 3,
            //        NumberOfEmployers = 2,
            //    },
            //    new AggregatedEmployerRequest
            //    {
            //        StandardReference = "ST0002",
            //        StandardTitle = "Aerospace engineer",
            //        StandardSector = "Engineering",
            //        StandardLevel = 3,
            //        NumberOfApprentices = 5,
            //        NumberOfEmployers = 1,
            //    },
            //};

            //_mediatorMock.Setup(m => m.Send(It.IsAny<GetAggregatedEmployerRequestsQuery>(), default)).ReturnsAsync(aggregatedRequests);

            //// Act
            //var result = await _mediator.GetViewAggregatedEmployerRequestsViewModel();

            //// Assert
            //result.Should().NotBeNull();
            //result.AggregatedEmployerRequests.Should().HaveCount(2);
            //result.RequestCount.Should().Be(2);
        }
    }
}
