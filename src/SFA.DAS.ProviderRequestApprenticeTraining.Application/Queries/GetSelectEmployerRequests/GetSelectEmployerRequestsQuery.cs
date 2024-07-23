﻿using MediatR;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests
{
    public class GetSelectEmployerRequestsQuery : IRequest<GetSelectEmployerRequestsResult>
    {
        public string StandardReference { get; set; }
        public long Ukprn { get; set; }

        public GetSelectEmployerRequestsQuery(long ukprn, string standardReference)
        {
            Ukprn = ukprn;
            StandardReference = standardReference;
        }
    }
}
