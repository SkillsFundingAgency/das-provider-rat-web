using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest
{
    public class SelectEmployerRequestsViewModelTests
    {

        public void BackRoute_ShouldReturnCheckAnswersRoute_When_BackToCheckAnswersTrue()
        {
            // Arrange
            var viewModel = new SelectEmployerRequestsViewModel
            {
                BackToCheckAnswers = true,
            };

            // Act
            var backRoute = viewModel.BackRoute;

            // Assert
            backRoute.Should().Be(EmployerRequestController.CheckYourAnswersRouteGet);
        }

        [Test]
        public void BackRoute_ShouldReturnActiveRoute_When_BackToCheckAnswersFalse()
        {
            // Arrange
            var viewModel = new SelectEmployerRequestsViewModel
            {
                BackToCheckAnswers = false,
            };

            // Act
            var backRoute = viewModel.BackRoute;

            // Assert
            backRoute.Should().Be(EmployerRequestController.ActiveRouteGet);
        }
    }
}
