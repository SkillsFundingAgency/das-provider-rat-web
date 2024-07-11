using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers
{
    [ExcludeFromCodeCoverage]
    public class ProviderAccountController : ControllerBase
    {
        #region Routes
        // exact route name is required by the SFA.DAS.Provider.Shared.UI menu link
        public const string ProviderSignOutRouteGet = "provider-signout";
        #endregion Routes

        public ProviderAccountController()
        {
        }

        [HttpGet("sign-out", Name = ProviderSignOutRouteGet)]
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