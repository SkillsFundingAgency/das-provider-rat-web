﻿using SFA.DAS.ProviderRequestApprenticeTraining.Web.Authorization;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUkprn(this ClaimsPrincipal user)
        {
            return user.FindFirst(ProviderClaims.ProviderUkprn)?.Value;
        }

        public static string GetEmailAddress(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ProviderClaims.Email) ?? user.FindFirstValue("email");
        }
    }
}