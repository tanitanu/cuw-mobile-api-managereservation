using System;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable]
    public class Baggage
    {
        public string BagTerminal { get; set; } = string.Empty;
        public string BagClaimUnit { get; set; } = string.Empty;
        public bool HasBagLocation { get; set; }

    }
}
