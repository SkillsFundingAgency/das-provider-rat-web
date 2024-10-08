using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization
{
    [ExcludeFromCodeCoverage]
    public static class ProviderClaims
    {
        public static string ProviderUkprn => "http://schemas.portal.com/ukprn";
        public static string DisplayName => "http://schemas.portal.com/displayname";
        public static string Service => "http://schemas.portal.com/service";
        public static string Email => "email";
        public static string GivenName => "given_name";
        public static string Sub => "sub";
    }
}