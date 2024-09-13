using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;


namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.UnitTests.Authorization
{
    public class MinimumServiceClaimAuthorizationHandlerTests
    {

        [Theory]
        [TestCaseSource(nameof(UsersWithContributorOrAbovePermissions))]
        public async Task Given_HandleRequirementAsync_And_UserHasPermission_SucceedsRequirement(ClaimsPrincipal user)
        {
            // Arrange
            var requirement = new MinimumServiceClaimRequirement(ServiceClaim.DAC);

            var context = new AuthorizationHandlerContext(new[] { requirement }, user, null);

            // Act
            var handler = new MinimumServiceClaimAuthorizationHandler();

            // Act
            await handler.HandleAsync(context);

            // Assert
            context.HasSucceeded.Should().BeTrue();
        }



        [Theory]
        [TestCaseSource(nameof(UsersWithoutContributorOrAbovePermissions))]
        public async Task Given_HandleRequirementAsync_And_UserDoesNotHavePermission_FailsRequirement(ClaimsPrincipal user)
        {
            // Arrange
            var requirement = new MinimumServiceClaimRequirement(ServiceClaim.DAC);

            var context = new AuthorizationHandlerContext(new[] { requirement }, user, null);

            // Act
            var handler = new MinimumServiceClaimAuthorizationHandler();

            // Act
            await handler.HandleAsync(context);

            // Assert
            context.HasSucceeded.Should().BeFalse();
        }

        public static IEnumerable<ClaimsPrincipal> UsersWithContributorOrAbovePermissions =>
            new List<ClaimsPrincipal>
            {
                new ClaimsPrincipal(new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "Test User"),
                        new Claim(ProviderClaims.Service, ServiceClaim.DAV.ToString()),
                        new Claim(ProviderClaims.Service, ServiceClaim.DAC.ToString()),
                        new Claim(ProviderClaims.Service, ServiceClaim.DAB.ToString()),
                        new Claim(ProviderClaims.Service, ServiceClaim.DAA.ToString())
                    }
                )),
                new ClaimsPrincipal(new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "Test User"),
                        new Claim(ProviderClaims.Service, ServiceClaim.DAC.ToString())
                    }
                )),
                new ClaimsPrincipal(new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "Test User"),
                        new Claim(ProviderClaims.Service, ServiceClaim.DAB.ToString())
                    }
                )),
                new ClaimsPrincipal(new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "Test User"),
                        new Claim(ProviderClaims.Service, ServiceClaim.DAA.ToString())
                    }
                )),

            };


        public static IEnumerable<ClaimsPrincipal> UsersWithoutContributorOrAbovePermissions =>
            new List<ClaimsPrincipal>
            {
                new ClaimsPrincipal(new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "Test User"),
                        new Claim(ProviderClaims.Service, ServiceClaim.DAV.ToString())
                    }
                )),
                new ClaimsPrincipal(new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "Test User")
                    }
                ))

            };
    }
}

