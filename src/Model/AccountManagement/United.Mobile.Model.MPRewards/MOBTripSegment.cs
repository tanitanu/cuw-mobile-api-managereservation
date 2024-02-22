using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBTripSegment
    {
        private string marketingCarrier;
        private string operatingCarrier;
        private string flightNumber = string.Empty;
        private MOBAirport departure;
        private MOBAirport arrival;
        private string scheduledDepartureDate = string.Empty;
        private string scheduledDepartureDateFormated = string.Empty;
        private string serviceClassDescription;
        private string codeshareFlightNumber;
        private string cscc;
        private string csfn;
        private bool isCrossFleet;
        private string crossFleetCOFlightNumber;
        private bool isCheckInWindow = false;
        private string checkInWindowText = string.Empty;
        private int segmentIndex;
        private bool cogStop;
        private bool isELF = false;
        private bool isIBE;
        private string epaMessageTitle = string.Empty;
        private bool showEPAMessage = false;
        private string carrierCode = string.Empty;
        private string serviceClass = string.Empty;
        private string operatingCarrierDescription = string.Empty;
        private string productCode;
        private string fareBasisCode;
        private int originalSegmentNumber;
        private int legIndex;
        private string continueButtonText;
        private string bundleProductCode;
        private bool showInterlineAdvisoryMessage;
        private string interlineAdvisoryMessage;
        private string interlineAdvisoryTitle;
        private string interlineAdvisoryAlertTitle;
        private bool isCheckedIn = false;
        private bool isAllPaxCheckedIn;

        public string InterlineAdvisoryAlertTitle
        {
            get { return interlineAdvisoryAlertTitle; }
            set { interlineAdvisoryAlertTitle = value; }
        }
        public string BundleProductCode
        {
            get { return bundleProductCode; }
            set { bundleProductCode = value; }
        }

        public string ContinueButtonText
        {
            get { return this.continueButtonText; }
            set { this.continueButtonText = value; }
        }

        public string EPAMessageTitle
        {
            get { return this.epaMessageTitle; }
            set { this.epaMessageTitle = value; }
        }

        private string epaMessage = string.Empty;

        public string EPAMessage
        {
            get { return this.epaMessage; }
            set { this.epaMessage = value; }
        }
        
        public bool ShowEPAMessage
        {
            get { return this.showEPAMessage; }
            set { this.showEPAMessage = value; }
        }

        public int SegmentIndex
        {
            get
            {
                return this.segmentIndex;
            }
            set
            {
                this.segmentIndex = value;
            }
        }

        public string MarketingCarrier
        {
            get
            {
                return this.marketingCarrier;
            }
            set
            {
                this.marketingCarrier = value;
            }
        }

        public string OperatingCarrier
        {
            get
            {
                return this.operatingCarrier;
            }
            set
            {
                this.operatingCarrier = value;
            }
        }

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

        public MOBAirport Departure
        {
            get
            {
                return this.departure;
            }
            set
            {
                this.departure = value;
            }
        }

        public MOBAirport Arrival
        {
            get
            {
                return this.arrival;
            }
            set
            {
                this.arrival = value;
            }
        }

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

        public bool IsCrossFleet
        {
            get { return this.isCrossFleet; }
            set { this.isCrossFleet = value; }
        }

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

        public bool IsCheckInWindow
        {
            get
            {
                return this.isCheckInWindow;
            }
            set
            {
                this.isCheckInWindow = value;
            }
        }

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

        public bool COGStop
        {
            get
            {
                return this.cogStop;
            }
            set
            {
                this.cogStop = value;
            }
        }

        public bool IsELF
        {
            get
            {
                return (this.MarketingCarrier == "UA" && this.ServiceClass == "N");
            }
        }

        public bool IsIBE
        {
            get { return isIBE; }
            set { isIBE = value; }
        }

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

        public string ServiceClass
        {
            get
            {
                return serviceClass;
            }

            set
            {
                serviceClass = string.IsNullOrEmpty(value)?string.Empty : value.Trim();
            }
        }

        public string OperatingCarrierDescription
        {
            get { return operatingCarrierDescription; }
            set { operatingCarrierDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }

        public string FareBasisCode
        {
            get { return fareBasisCode; }
            set { fareBasisCode = value; }
        }
        public int OriginalSegmentNumber
        {
            get { return originalSegmentNumber; }
            set { originalSegmentNumber = value; }
        }
        public int LegIndex
        {
            get { return legIndex; }
            set { legIndex = value; }
        }

        public bool ShowInterlineAdvisoryMessage
        {
            get { return showInterlineAdvisoryMessage; }
            set { showInterlineAdvisoryMessage = value; }
        }
        public string InterlineAdvisoryMessage
        {
            get { return interlineAdvisoryMessage; }
            set { interlineAdvisoryMessage = value; }
        }
        public string InterlineAdvisoryTitle
        {
            get { return interlineAdvisoryTitle; }
            set { interlineAdvisoryTitle = value; }
        }

        private string interlineAdvisoryDeepLinkURL;
        public string InterlineAdvisoryDeepLinkURL
        {
            get { return interlineAdvisoryDeepLinkURL; }
            set { interlineAdvisoryDeepLinkURL = value; }
        }

        public bool IsCheckedIn { get { return this.isCheckedIn; } set { this.isCheckedIn = value; } }
        public bool IsAllPaxCheckedIn { get { return isAllPaxCheckedIn; } set { isAllPaxCheckedIn = value; } }

        //private MOBRedirectButton oARedirectButton;
        //public MOBRedirectButton OARedirectButton {
        //    get { return oARedirectButton; }
        //    set { oARedirectButton = value; }
        //}
    }
}
