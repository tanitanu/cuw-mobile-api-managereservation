using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.BagCalculator
{
    [Serializable]
    public class DOTBaggageTravelerInfo
    {
        public string Id { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime TicketingDate { get; set; }
        public string TicketingDateString { get; set; } = string.Empty;
        public DOTBaggageLoyalty Loyalty { get; set; }
    }
}
