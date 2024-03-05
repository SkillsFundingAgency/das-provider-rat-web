using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.ProviderRequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.HealthChecks
{
    public class ApiHealthCheck : IHealthCheck
    {
        private readonly IProviderRequestApprenticeTrainingOuterApi _outerApi;

        public ApiHealthCheck(IProviderRequestApprenticeTrainingOuterApi outerApi)
        {
            _outerApi = outerApi;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            var description = "Ping of Provider Request Apprentice Training outer API";

            try
            {
                await _outerApi.Ping();
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(description, ex);
            }

            return HealthCheckResult.Healthy(description, new Dictionary<string, object>());
        }
    }
}