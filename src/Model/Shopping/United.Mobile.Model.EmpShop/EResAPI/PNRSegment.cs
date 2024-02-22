using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    public class PNRSegment
    {
        public string ArrivalAirportCode { get; set; } = string.Empty;
        public AvailableRoute AvailableRoutes { get; set; } = null;
        public string DepartureAirportCode { get; set; } = string.Empty;
        public Airport ArrivalAirport { get; set; } = null;
        public FlightDateTime ArrivalDateTime { get; set; } = null;
        public MOBTypeOption Cabin { get; set; } = null;
        public string CarrierCode { get; set; } = string.Empty;
        public TravelTime ConnectTime { get; set; } = null;
        public Airport DepartureAirport { get; set; } = null;
        public FlightDateTime DepartureDateTime { get; set; } = null;
        public string FlightNumber { get; set; } = string.Empty;
        public FlightStatusSegment FlightStatus { get; set; } = new FlightStatusSegment();
        public PassRiderListResponse PassengerList { get; set; } = null;
        public TravelTime FlightTime { get; set; } = null;
        public bool IsActive { get; set; } = false;
        public bool IsLifted { get; set; } = false;
        public bool IsUsed { get; set; } = false;
        public string OperatingCarrier { get; set; } = string.Empty;
        public string PassClass { get; set; } = string.Empty;
        public int SegmentIndex { get; set; } = 0;
        public int TripIndex { get; set; } = 0;
        public bool ShowAmenities { get; set; } = false;
        public bool ShowFlightStatus { get; set; } = false;
        public bool ShowCancelSegment { get; set; } = false;
        public bool ShowChangeSegment { get; set; } = false;
        public bool ShowFullPassRiderList { get; set; } = false;
    }
}
