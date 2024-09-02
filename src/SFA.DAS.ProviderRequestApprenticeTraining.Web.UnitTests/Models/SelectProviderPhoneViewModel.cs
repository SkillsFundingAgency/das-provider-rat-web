using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest
{
    public class SelectProviderPhoneViewModelTests
    {
        public void BackRoute_ShouldReturnCheckAnswersRoute_When_BackToCheckAnswersTrue()
        {
            // Arrange
            var viewModel = new SelectProviderPhoneViewModel
            {
                BackToCheckAnswers = true,
                HasSingleEmail = true,
                HasSinglePhoneNumber = true,
            };

            // Act
            var backRoute = viewModel.BackRoute;

            // Assert
            backRoute.Should().Be(EmployerRequestController.CheckYourAnswersRouteGet);
        }

        [Test]
        public void BackRoute_ShouldReturnSelectEmailRoute_When_BackToCheckAnswersFalse_And_HasSingleEmailFalse()
        {
            // Arrange
            var viewModel = new SelectProviderPhoneViewModel
            {
                BackToCheckAnswers = false,
                HasSingleEmail = false,
            };

            // Act
            var backRoute = viewModel.BackRoute;

            // Assert
            backRoute.Should().Be(EmployerRequestController.SelectProviderEmailRouteGet);
        }

        [Test]
        public void BackRoute_ShouldReturnSelectRequestsToContact_When_BackToCheckAnswersFalse_And_HasSingleEmailTrue()
        {
            // Arrange
            var viewModel = new SelectProviderPhoneViewModel
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
