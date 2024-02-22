using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{

    [Serializable]
    public class DOTBaggageFlightSegment
    {
        private string departureAirportCode = string.Empty;
        private string arrivalAirportCode = string.Empty;
        private DateTime departureDate;
        private string departureDateString = string.Empty;
        private string operatingAirline = string.Empty;
        private string marketingAirline = string.Empty;
        private string segmentNumber = string.Empty;
        private string classOfService = string.Empty;

        public string DepartureAirportCode
        {
            get { return this.departureAirportCode; }
            set { this.departureAirportCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string ArrivalAirportCode
        {
            get { return this.arrivalAirportCode; }
            set { this.arrivalAirportCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public DateTime DepartureDate
        {
            get
            {
                return this.departureDate;
            }
            set
            {
                this.departureDate = value;
            }
        }

        public string DepartureDateString
        {
            get { return this.departureDateString; }
            set { this.departureDateString = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string OperatingAirline
        {
            get { return this.operatingAirline; }
            set { this.operatingAirline = string.IsNullOrEmpty(value) ? "UA" : value.Trim(); }
        }

        public string MarketingAirline
        {
            get { return this.marketingAirline; }
            set { this.marketingAirline = string.IsNullOrEmpty(value) ? "UA" : value.Trim(); }
        }

        public string SegmentNumber
        {
            get { return this.segmentNumber; }
            set { this.segmentNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string ClassOfService
        {
            get { return this.classOfService; }
            set { this.classOfService = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
