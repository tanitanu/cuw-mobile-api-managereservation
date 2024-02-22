using System;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBBagFeesPerSegment
    {
        private string flightTravelDate = string.Empty;
        private string originAirportCode = string.Empty;
        private string originAirportName = string.Empty;
        private string destinationAirportCode = string.Empty;
        private string destinationAirportName = string.Empty;
        private string firstBagFee = string.Empty;
        private string regularFirstBagFee = string.Empty;
        private string secondBagFee = string.Empty;
        private string regularSecondBagFee = string.Empty;
        private string weightPerBag = string.Empty;
        private bool isFirstBagFree = false;
        private bool isSecondBagFree = false;
        private string cabinName;

        public string FlightTravelDate
        {
            get { return this.flightTravelDate; }
            set { this.flightTravelDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string OriginAirportCode
        {
            get { return this.originAirportCode; }
            set { this.originAirportCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string OriginAirportName
        {
            get { return this.originAirportName; }
            set { this.originAirportName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string DestinationAirportCode
        {
            get { return this.destinationAirportCode; }
            set { this.destinationAirportCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string DestinationAirportName
        {
            get { return this.destinationAirportName; }
            set { this.destinationAirportName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string FirstBagFee
        {
            get { return this.firstBagFee; }
            set { this.firstBagFee = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string RegularFirstBagFee
        {
            get { return this.regularFirstBagFee; }
            set { this.regularFirstBagFee = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string SecondBagFee
        {
            get { return this.secondBagFee; }
            set { this.secondBagFee = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string RegularSecondBagFee
        {
            get { return this.regularSecondBagFee; }
            set { this.regularSecondBagFee = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string WeightPerBag
        {
            get { return this.weightPerBag; }
            set { this.weightPerBag = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public bool IsFirstBagFree
        {
            get { return this.isFirstBagFree; }
            set { this.isFirstBagFree = value; }
        }

        public bool IsSecondBagFree
        {
            get { return this.isSecondBagFree; }
            set { this.isSecondBagFree = value; }
        }

        public string CabinName
        {
            get { return cabinName; }
            set { cabinName = value; }
        }
    }
}
