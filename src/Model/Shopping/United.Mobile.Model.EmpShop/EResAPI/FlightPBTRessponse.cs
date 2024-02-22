using System;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class FlightPBTRessponse
    {
        public bool IsAllowedToSelectFlight { get; set; }
        public AvailableRoute AvailableRoutes { get; set; }
        public ErrorInfo Error { get; set; }
        public string LastCallDateTime { get; set; }
        public string ServerName { get; set; }
        public string Status { get; set; }
        public string TransactionID { get; set; }
        public EResAlert BaseAlert { get; set; }
        public string TransferMessage { get; set; }
    }
}
