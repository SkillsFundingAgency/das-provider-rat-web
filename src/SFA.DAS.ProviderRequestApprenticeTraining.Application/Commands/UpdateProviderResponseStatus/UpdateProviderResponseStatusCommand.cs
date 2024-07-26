﻿using MediatR;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.UpdateProviderResponseStatus
{
    public class UpdateProviderResponseStatusCommand : IRequest<bool>
    {
        public long Ukprn { get; set; }
        public List<Guid> EmployerRequestIds { get; set; }
        public int ProviderResponseStatus { get; set; }
    }
}