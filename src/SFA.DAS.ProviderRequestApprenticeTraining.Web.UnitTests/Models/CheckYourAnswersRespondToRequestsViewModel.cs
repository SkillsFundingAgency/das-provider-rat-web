using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest
{
    public class CheckYourAnswersRespondToRequestsViewModelTests
    {
        public void BackRoute_ShouldReturnEmailRoute_When_HasSingleEmailFalse_And_HasSinglePhoneTrue()
        {
            // Arrange
            var viewModel = new CheckYourAnswersRespondToRequestsViewModel
            {
                HasSingleEmail = false,
                HasSinglePhone = true,
            };

            // Act
            var backRoute = viewModel.BackRoute;

            // Assert
            backRoute.Should().Be(EmployerRequestController.SelectProviderEmailRouteGet);
        }

        [Test]
        public void BackRoute_ShouldReturnPhoneRoute_When_HasSinglePhoneFalse()
        {
            // Arrange
            var viewModel = new CheckYourAnswersRespondToRequestsViewModel
            {
                HasSingleEmail = false,
                HasSinglePhone = false,
            };

            // Act
            var backRoute = viewModel.BackRoute;

            // Assert
            backRoute.Should().Be(EmployerRequestController.SelectProviderPhoneRouteGet);
        }

        [Test]
        public void BackRoute_ShouldReturnSelectRequestsRoute_When_HasSingleEmailTrue_And_HasSinglePhoneTrue()
        {
            // Arrange
            var viewModel = new CheckYourAnswersRespondToRequestsViewModel
            {
                HasSingleEmail = true,
                HasSinglePhone = true,
            };

            // Act
            var backRoute = viewModel.BackRoute;

            // Assert
            backRoute.Should().Be(EmployerRequestController.SelectRequestsToContactRouteGet);
        }

    }
}
