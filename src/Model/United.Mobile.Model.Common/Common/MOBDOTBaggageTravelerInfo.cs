using System;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBDOTBaggageTravelerInfo
    {
        public string Id { get; set; } = string.Empty;

        public string GivenName { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public DateTime TicketingDate { get; set; } 

        public string TicketingDateString { get; set; } = string.Empty;

        public MOBDOTBaggageLoyalty Loyalty { get; set; }
    }
}
