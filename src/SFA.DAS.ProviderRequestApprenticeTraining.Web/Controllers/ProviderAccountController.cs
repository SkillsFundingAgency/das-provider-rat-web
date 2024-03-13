using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers
{
    [ExcludeFromCodeCoverage]
    public class ProviderAccountController : ControllerBase
    {
        public ProviderAccountController()
        {
        }

        [HttpGet("sign-out", Name = RouteNames.ProviderSignOut)]
        public IActionResult ProviderSignOut()
        {
            return SignOut(
                new AuthenticationProperties
                {
                    RedirectUri = string.Empty,
                    AllowRefresh = true
                },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}