using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBEmpBookingSegment
    {
        //89|IAH|EWR|0600|1025|10/15/2010|0205|F'
        //A  B   C   D    E    F          G    H
        //A = Flight Number
        //B = Origin
        //C = Destination
        //D = Depart time
        //E = Arrival Time
        //F = Departure Date
        //G = travel time in minutes
        //H = First Class bucket
        private string fln = string.Empty;
        private MOBEmpAirport origin;
        private MOBEmpAirport destination;
        private string departTime = string.Empty;
        private string arrivalTime = string.Empty;
        private string departDate = string.Empty;
        private string arrivalDate = string.Empty;
        private MOBEmpTravelTime travelTime;
        private string firstClassBucket = string.Empty;
        private string marketingCarrier = string.Empty;
        private string operatingCarrier = string.Empty;
        private int segmentIndex;
        private bool isBusinessFirstEligible;
        private bool isBusinessCoachEligible;
        private MOBEmpDEI dei;
        private MOBTypeOption cabin;
        private string cvd;
        private int cct;
        private MOBEmpPBTType capacity;
        private MOBEmpPSCost psCost;

        
        public string FlightNumber
        {
            get { return this.fln; }
            set { this.fln = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public MOBEmpAirport Origin
        {
            get { return this.origin; }
            set { this.origin = value; }
        }

        public MOBEmpAirport Destination
        {
            get { return this.destination; }
            set { this.destination = value; }
        }

        public string DepartTime
        {
            get { return this.departTime; }
            set { this.departTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string ArrivalTime
        {
            get { return this.arrivalTime; }
            set { this.arrivalTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string ArrivalDate
        {
            get { return this.arrivalDate; }
            set { this.arrivalDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string DepartDate
        {
            get { return this.departDate; }
            set { this.departDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public MOBEmpTravelTime TravelTime
        {
            get { return this.travelTime; }
            set { this.travelTime = value; }
        }

        public string FirstClassBucket
        {
            get { return this.firstClassBucket; }
            set { this.firstClassBucket = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string MarketingCarrier
        {
            get { return this.marketingCarrier; }
            set { this.marketingCarrier = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string OperatingCarrier
        {
            get { return this.operatingCarrier; }
            set { this.operatingCarrier = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public int SegmentIndex
        {
            get { return this.segmentIndex; }
            set { this.segmentIndex = value; }
        }

        public bool IsBusinessFirstEligible
        {
            get { return isBusinessFirstEligible; }
            set { isBusinessFirstEligible = value; }
        }

        public bool IsBusinessCoachEligible
        {
            get { return isBusinessCoachEligible; }
            set { isBusinessCoachEligible = value; }
        }

        public MOBEmpDEI DEI
        {
            get { return dei; }
            set { dei = value; }
        }

        public MOBTypeOption Cabin
        {
            get { return cabin; }
            set { cabin = value; }
        }

        public string CurrentViewDate
        {
            get { return this.cvd; }
            set { this.cvd = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public int CabinCount
        {
            get { return this.cct; }
            set { this.cct = value; }
        }

        public MOBEmpPSCost PSCost
        {
            get { return psCost; }
            set { psCost = value; }
        }
        public MOBEmpPBTType Capacity
        {
            get { return this.capacity; }
            set { this.capacity = value; }
        }
    }
}


