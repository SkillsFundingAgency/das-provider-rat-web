﻿namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses
{
    public class SelectEmployerRequestResponse : IEmployerRequestResponse
    {
        public Guid EmployerRequestId { get; set; }
        public List<string> Locations { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public bool BlockRelease { get; set; }
        public bool DayRelease { get; set; }
        public string DateOfRequest { get; set; }
        public int NumberOfApprentices { get; set; }
        public bool IsNew { get; set; }
        public bool IsContacted { get; set; }
        public string DateContacted { get; set; }
    }
}
