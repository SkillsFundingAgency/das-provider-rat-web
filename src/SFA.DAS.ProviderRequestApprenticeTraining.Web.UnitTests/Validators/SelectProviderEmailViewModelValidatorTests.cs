using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Validators;
using SFA.DAS.Testing.AutoFixture;


namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.UnitTests.Validators
{
    [TestFixture]
    public class SelectProviderEmailViewModelValidatorTests
    {
        private SelectProviderEmailViewModelValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new SelectProviderEmailViewModelValidator();
        }

        [Test]
        public async Task Should_Have_Error_When_SelectedEmail_Is_Empty()
        {
            var model = new SelectProviderEmailViewModel { SelectedEmail = string.Empty };
            var result = await _sut.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.SelectedEmail);
        }

        [Test]
        public async Task Should_Not_Have_Error_When_SelectedEmail_Is_Valid()
        {
            var model = new SelectProviderEmailViewModel { SelectedEmail = "anytext@email.com" };
            var result = await _sut.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(x => x.SelectedEmail);
        }
    }
}
