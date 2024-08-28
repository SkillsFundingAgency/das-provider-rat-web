using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.EmployerRequest;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Validators;
using SFA.DAS.Testing.AutoFixture;


namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.UnitTests.Validators
{
    [TestFixture]
    public class SelectEmployerRequestsViewModelValidatorTests
    {
        private SelectEmloyerRequestsViewModelValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new SelectEmloyerRequestsViewModelValidator();
        }

        [Test]
        public async Task Should_Have_Error_When_SelectedRequests_Is_Empty()
        {
            var model = new EmployerRequestsToContactViewModel { SelectedRequests = new List<Guid>()};
            var result = await _sut.TestValidateAsync(model);
            result.ShouldHaveValidationErrorFor(x => x.SelectedRequests);
        }

        [Test]
        public async Task Should_Not_Have_Error_When_SelectedEmail_Is_Valid()
        {
            var model = new EmployerRequestsToContactViewModel { SelectedRequests = new List<Guid> { Guid.NewGuid()} };
            var result = await _sut.TestValidateAsync(model);
            result.ShouldNotHaveValidationErrorFor(x => x.SelectedRequests);
        }
    }
}
