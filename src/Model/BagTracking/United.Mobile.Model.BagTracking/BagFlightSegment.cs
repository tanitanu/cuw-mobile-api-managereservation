using System;

namespace United.Mobile.Model.BagTracking
{
    [Serializable]
    public class BagFlightSegment
    {
        private string flightOriginationDate = string.Empty;
        private string arrivalRouteTypeCode = string.Empty;
        private string departureRouteTypeCode = string.Empty;

        public string ArrivalRouteTypeCode
        {
            get
            {
                return arrivalRouteTypeCode;
            }
            set
            {
                this.arrivalRouteTypeCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public BagAction BagAction { get; set; }
        public BagStatus BagStatus { get; set; }
        public string DepartureRouteTypeCode
        {
            get
            {
                return departureRouteTypeCode;
            }
            set
            {
                this.departureRouteTypeCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string FlightOriginationDate
        {
            get
            {
                return flightOriginationDate;
            }
            set
            {
                this.flightOriginationDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public Segment Segment { get; set; }

    }
}
