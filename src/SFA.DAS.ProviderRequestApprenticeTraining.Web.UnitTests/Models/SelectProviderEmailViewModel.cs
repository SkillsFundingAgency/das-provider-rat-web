using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest
{
    public class SelectProviderEmailViewModelTests
    {

        public void BackRoute_ShouldReturnCheckAnswersRoute_When_BackToCheckAnswersTrue()
        {
            // Arrange
            var viewModel = new SelectProviderEmailViewModel
            {
                BackToCheckAnswers = true,
            };

            // Act
            var backRoute = viewModel.BackRoute;

            // Assert
            backRoute.Should().Be(EmployerRequestController.CheckYourAnswersRouteGet);
        }

        [Test]
        public void BackRoute_ShouldReturnSelectRequestsToContactRoute_When_BackToCheckAnswersFalse()
        {
            // Arrange
            var viewModel = new SelectProviderEmailViewModel
            {
                BackToCheckAnswers = false,
                HasSingleEmail = true,
            };

            // Act
            var backRoute = viewModel.BackRoute;

            // Assert
            backRoute.Should().Be(EmployerRequestController.SelectRequestsToContactRouteGet);
        } 
    }
}
