using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace United.Mobile.Model.BagTracking
{
    [Serializable]
    public class BagsDetails
    {

        private Passenger passenger;

        public List<Bag> Bags { get; set; }

        public Passenger Passenger { get; set; }


        public List<DisplayBagTrackDetails> DisplayBagTrackDetails { get; set; }

        public BagsDetails()
        {
            Bags = new List<Bag>();
            DisplayBagTrackDetails = new List<DisplayBagTrackDetails>();

        }

    }
    [Serializable]
    public class DisplayBagTrackDetails
    {
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string BagTagNumber { get; set; } = string.Empty;
        public bool BagRerouted { get; set; }
        public string ShowBagStatus { get; set; } = string.Empty;
        public BagTrackStatusType ShowBagTrackStatusType { get; set; }
        public List<DisplayBagTrackStatus> DisplayBagTrackStatuses { get; set; }
        public string BagClaimUrl { get; set; } = string.Empty;

        public string BagClaimIndicator { get; set; } = string.Empty;
        public string ClaimButtonText { get; set; } = string.Empty;

        private string boltFileReferenceNumber { get; set; } = string.Empty;
        public string BoltFileReferenceNumber
        {
            get
            {
                return boltFileReferenceNumber;
            }
            set
            {
                this.boltFileReferenceNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public DisplayBagTrackDetails()
        {
            DisplayBagTrackStatuses = new List<DisplayBagTrackStatus>();
        }

    }

    [Serializable]
    public class DisplayBagTrackStatus
    {
        // Ex: Checked at STL / UA1234/STL to ORD
        public string BagFlightSegmentInfo { get; set; } 
        // Received at 5:00 am / Arrived at 6:12 am // Bag is on an earlier flight
        public string BagStatusInfo { get; set; } = string.Empty;
        public BagTrackStatusInfoColor BagStatusInfoColor { get; set; }
        public BagTrackStatusLine BagStatusInfoLine { get; set; }
        public BagTrackStatusImage BagTrackStatusImage { get; set; }
        public BagTrackStatusType BagTrackStatusType { get; set; }

        // if MOBBagTrackStatusType = Alert or Bag Info then show this message "Your bag is on earlier flight ...." or "Your bag will be placed on a different flight ..." or "When you arrive in Tokyo, please see a United baggage representative..."
        public string BagStatusInfoDetails { get; set; } 

        // if the last flight segment is not MOBBagTrackStatusType != Arrived then need to send the Flight Request for Flight Status.
        public DisplayBagTrackFLIFORequest FlightStatusRequest { get; set; }

    }

    [Serializable]
    public enum BagTrackStatusInfoColor
    {
        [EnumMember(Value = "Default")]
        Default,
        [EnumMember(Value = "Red")]
        Red,
        [EnumMember(Value = "Green")]
        Green
    }

    [Serializable]
    public enum BagTrackStatusLine
    {
        [EnumMember(Value = "Default")] ///for full line
        Default,
        [EnumMember(Value = "Up")]
        Up,
        [EnumMember(Value = "Down")]
        Down,
        [EnumMember(Value = "Empty")]
        Empty
    }

    [Serializable]
    public enum BagTrackStatusType
    {
        [EnumMember(Value = "None")]
        None,
        [EnumMember(Value = "Received")]
        Received,
        [EnumMember(Value = "Arrived")]
        Arrived,
        [EnumMember(Value = "Alert")]
        Alert,
        [EnumMember(Value = "AlertInfo")]
        AlertInfo,
        [EnumMember(Value = "AlertLine")]
        AlertLine,
        [EnumMember(Value = "BagEarlyAlert")] // EX: Bag Icon Image should be displayed folling with this sample message "Bag is on an earlier flight" 
        BagEarlyAlert,
        [EnumMember(Value = "BagEarlyAlertLine")]
        BagEarlyAlertLine,
        [EnumMember(Value = "BagEarlyAlertInfo")]
        BagEarlyAlertInfo,
        [EnumMember(Value = "InFlight")]
        InFlight,
        [EnumMember(Value = "Departs")]
        Departs,
        [EnumMember(Value = "Delayed")]
        Delayed,
        [EnumMember(Value = "Diverted")]
        Diverted,
        [EnumMember(Value = "Cancelled")]
        Cancelled,
        [EnumMember(Value = "FlightStatus")]
        FlightStatus,
        [EnumMember(Value = "BaggageClaim")]
        BaggageClaim
    }

    [Serializable]
    public enum BagTrackStatusImage
    {
        [EnumMember(Value = "GreenCheck")]
        GreenCheck,
        [EnumMember(Value = "GreyCheck")]
        GreyCheck,
        [EnumMember(Value = "GreySmallCheck")]
        GreySmallCheck,
        [EnumMember(Value = "Alert")]
        Alert,
        [EnumMember(Value = "Info")]
        Info,
        [EnumMember(Value = "EmptyCircle")]
        EmptyCircle,
        [EnumMember(Value = "GreenSmallCheck")]
        GreenSmallCheck
    }

    [Serializable]
    public class DisplayBagTrackFLIFORequest
    {
        public string OriginAirportCode { get; set; } = string.Empty;
        public string DestinationAirportCode { get; set; } = string.Empty;
        public int FlightNumber { get; set; }
        public string FlightDate { get; set; } = string.Empty;

    }

    [Serializable]
    public class DisplayBagTrackAirportDetails
    {
        public string AirportCode { get; set; } = string.Empty;
        public string AirportInfo { get; set; } = string.Empty;
        public string AirportCityName { get; set; } = string.Empty;

    }
}


