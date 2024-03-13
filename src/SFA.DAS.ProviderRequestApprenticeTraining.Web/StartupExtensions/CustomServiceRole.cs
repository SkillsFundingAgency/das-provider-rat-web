using SFA.DAS.DfESignIn.Auth.Enums;
using SFA.DAS.DfESignIn.Auth.Interfaces;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.StartupExtensions
{
    [ExcludeFromCodeCoverage]
    public class CustomServiceRole : ICustomServiceRole
    {
        public string RoleClaimType => ProviderClaims.Service;

        // <inherit-doc/>
        public CustomServiceRoleValueType RoleValueType => CustomServiceRoleValueType.Code;
    }
}