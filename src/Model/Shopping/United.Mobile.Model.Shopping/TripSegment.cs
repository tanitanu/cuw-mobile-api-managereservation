using System;
using System.Xml.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class TripSegment
    {

        private string flightNumber = string.Empty;
        private string scheduledDepartureDate = string.Empty;
        private string scheduledDepartureDateFormated = string.Empty;
        private string serviceClassDescription;
        private string codeshareFlightNumber;
        private string cscc;
        private string csfn;
        private string crossFleetCOFlightNumber;
        private string checkInWindowText = string.Empty;
        private string carrierCode = string.Empty;
        private string serviceClass = string.Empty;
        private string operatingCarrierDescription = string.Empty;        
        private bool isAllPaxCheckedIn;
        private bool showSeatChange = true;

        public string InterlineAdvisoryAlertTitle { get; set; }

        public string BundleProductCode { get; set; } = string.Empty;

        public string ContinueButtonText { get; set; }

        public string EPAMessageTitle { get; set; } = string.Empty;

        private string epaMessage = string.Empty;

        public string EPAMessage { get; set; } = string.Empty;

        public bool ShowEPAMessage { get; set; }

        public int SegmentIndex { get; set; }

        public string MarketingCarrier { get; set; } = string.Empty;

        public string OperatingCarrier { get; set; } = string.Empty;

        public string FlightNumber
        {
            get
            {
                return this.flightNumber;
            }
            set
            {
                this.flightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBAirport Departure { get; set; }

        public MOBAirport Arrival { get; set; }

        public string ScheduledDepartureDate
        {
            get
            {
                return this.scheduledDepartureDate;
            }
            set
            {
                this.scheduledDepartureDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ScheduledDepartureDateFormated
        {
            get
            {
                return this.scheduledDepartureDateFormated;
            }
            set
            {
                this.scheduledDepartureDateFormated = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public Aircraft Equipment { get; set; }

        public string ServiceClassDescription
        {
            get
            {
                return this.serviceClassDescription;
            }
            set
            {
                this.serviceClassDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CodeshareFlightNumber
        {
            get
            {
                return this.codeshareFlightNumber;
            }
            set
            {
                this.codeshareFlightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
        public string CSCC
        {
            get
            {
                return this.cscc;
            }
            set
            {
                this.cscc = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string CSFN
        {
            get
            {
                return this.csfn;
            }
            set
            {
                this.csfn = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public bool IsCrossFleet { get; set; }

        public string CrossFleetCOFlightNumber
        {
            get
            {
                return this.crossFleetCOFlightNumber;
            }
            set
            {
                this.crossFleetCOFlightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private bool isCheckedIn = false;
        public bool IsCheckedIn { get { return this.isCheckedIn; } set { this.isCheckedIn = value; } }

        public bool IsCheckInWindow { get; set; }

        public string CheckInWindowText
        {
            get
            {
                return this.checkInWindowText;
            }
            set
            {
                this.checkInWindowText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool COGStop { get; set; }

        public bool IsELF
        {
            get
            {
                return (this.MarketingCarrier == "UA" && this.ServiceClass == "N");
            }
        }

        public bool IsIBE { get; set; }

        public string CarrierCode
        {
            get
            {
                return this.carrierCode;
            }
            set
            {
                this.carrierCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OperatingCarrierDescription
        {
            get { return operatingCarrierDescription; }
            set { operatingCarrierDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string ProductCode { get; set; } = string.Empty;

        public string FareBasisCode { get; set; } = string.Empty;
        public int OriginalSegmentNumber { get; set; } 
        public int LegIndex { get; set; } 

        public bool ShowInterlineAdvisoryMessage { get; set; } 
        public string InterlineAdvisoryMessage { get; set; }
        public string InterlineAdvisoryTitle { get; set; }

        public string ServiceClass
        {
            get
            {
                return serviceClass;
            }

            set
            {
                serviceClass = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string InterlineAdvisoryDeepLinkURL { get; set; }   
        public bool IsAllPaxCheckedIn { get { return isAllPaxCheckedIn; } set { isAllPaxCheckedIn = value; } }
        public bool ShowSeatChange
        {
            get { return this.showSeatChange; }
            set { this.showSeatChange = value; }
        }

    }
}
