using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Validators;
using SFA.DAS.Testing.AutoFixture;


namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.UnitTests.Validators
{
    [TestFixture]
    public class SelectPhoneViewModelValidatorTests
    {
        private SelectProviderPhoneViewModelValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new SelectProviderPhoneViewModelValidator();
        }

        [Test]
        public async Task Should_Have_Error_When_SelectedPhone_Is_Empty()
        {
            var model = new SelectProviderPhoneViewModel { SelectedPhoneNumber = string.Empty };
            var result = await _sut.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.SelectedPhoneNumber);
        }

        [Test]
        public async Task Should_Not_Have_Error_When_SelectedPhone_Is_Valid()
        {
            var model = new SelectProviderPhoneViewModel { SelectedPhoneNumber = "12346789" };
            var result = await _sut.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(x => x.SelectedPhoneNumber);
        }
    }
}
