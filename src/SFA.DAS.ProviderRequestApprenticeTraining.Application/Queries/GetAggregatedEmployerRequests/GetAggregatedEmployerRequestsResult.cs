using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests
{
    public class GetAggregatedEmployerRequestsResult
    {
        public List<AggregatedEmployerRequestResponse> AggregatedEmployerRequests { get; set; }
    }
}
