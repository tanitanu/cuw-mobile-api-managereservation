using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.PassRiderList
{
    public class EmpTravelerMisConnectRequest
    {
        public string Origin { get; set; } = string.Empty;

        public string Destination { get; set; } = string.Empty;

        public string FlightDate { get; set; } = string.Empty;

        public string FlightNumber { get; set; } = string.Empty;

        public string CarrierCode { get; set; } = string.Empty;

        public bool IsConnectingFlight { get; set; } = false;
        public string SessionId { get; set; }
    }
}
