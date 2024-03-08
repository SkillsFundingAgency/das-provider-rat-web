using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SFA.DAS.Provider.Shared.UI.Attributes;
using SFA.DAS.Provider.Shared.UI.Models;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Configuration;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Models.Error;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Controllers
{
    [HideNavigationBar(hideAccountHeader: false, hideNavigationLinks: true)]
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        private readonly ProviderRequestApprenticeTrainingWebConfiguration _configuration;
        private readonly ProviderSharedUIConfiguration _providerSharedUIConfiguration;

        public ErrorController(
            IOptions<ProviderRequestApprenticeTrainingWebConfiguration> configuration,
            IOptions<ProviderSharedUIConfiguration> providerSharedUIConfiguration)
        {
            _configuration = configuration?.Value;
            _providerSharedUIConfiguration = providerSharedUIConfiguration?.Value;
        }

        [Route("{statusCode?}")]
        public IActionResult Error(int? statusCode)
        {
            switch (statusCode)
            {
                case 403:
                    return View(statusCode.ToString(), new Error403ViewModel()
                    {
                        RoleRequestHelpLink = _configuration.RoleRequestHelpLink,
                        DashboardLink = _providerSharedUIConfiguration.DashboardUrl
                    }); ;
                case 404:
                    return View(statusCode.ToString());
                default:
                    return View();
            }
        }
    }
}