using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Validators;
using SFA.DAS.Testing.AutoFixture;


namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.UnitTests.Validators
{
    [TestFixture]
    public class CheckYourAnswersViewModelValidatorTests
    {
        private CheckYourAnswersViewModelValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CheckYourAnswersViewModelValidator();
        }

        [Test]
        public async Task Should_Have_Error_When_Email_Is_Empty()
        {
            var model = new CheckYourAnswersRespondToRequestsViewModel { Email = string.Empty };
            var result = await _sut.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Test]
        public async Task Should_Not_Have_Error_When_Email_Is_Valid()
        {
            var model = new CheckYourAnswersRespondToRequestsViewModel { Email = "anytext@email.com" };
            var result = await _sut.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Test]
        public async Task Should_Have_Error_When_Phone_Is_Empty()
        {
            var model = new CheckYourAnswersRespondToRequestsViewModel { Phone = string.Empty };
            var result = await _sut.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.Phone);
        }

        [Test]
        public async Task Should_Not_Have_Error_When_Phone_Is_Valid()
        {
            var model = new CheckYourAnswersRespondToRequestsViewModel { Phone = "123456" };
            var result = await _sut.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        }

        [Test]
        public async Task Should_Have_Error_When_SelectedRequests_Is_Empty()
        {
            var model = new CheckYourAnswersRespondToRequestsViewModel { SelectedRequests = new List<EmployerRequestViewModel>() };
            var result = await _sut.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.SelectedRequests);
        }

        [Test, MoqAutoData]
        public async Task Should_Not_Have_Error_When_SelectRequests_Is_NotEmpty(List<EmployerRequestViewModel> selectedRequests)
        {
            var model = new CheckYourAnswersRespondToRequestsViewModel { SelectedRequests = selectedRequests };
            var result = await _sut.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(x => x.SelectedRequests);
        }
    }
}
