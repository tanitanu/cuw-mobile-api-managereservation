using Newtonsoft.Json;
using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class Reshop
    {
        private string ancillaryRefundFormOfPayment;
        private string recordLocator;
        private string flowType;
        private string lastName;
        private string reviewChangeBackBtnText;
        private string fsrTitle;
        private string fsrChangeFeeTxt;
        private string rtiChangeCancelTxt;
        private string checkinSessionKey;
        private string fsrSubHeading;
        private MOBFutureFlightCredit ffcMessage;
        private bool isResidualFFCRAvailable;

        public bool IsResidualFFCRAvailable
        { get { return this.isResidualFFCRAvailable; } set { this.isResidualFFCRAvailable = value; } }
        public MOBFutureFlightCredit FFCMessage
        { get { return this.ffcMessage; } set { this.ffcMessage = value; } }
        public string RTIChangeCancelTxt
        { get { return this.rtiChangeCancelTxt; } set { this.rtiChangeCancelTxt = value; } }
        
        
        public string FsrSubHeading
        { get { return this.fsrSubHeading; } set { this.fsrSubHeading = value; } }

        public string RecordLocator
        {
            get { return this.recordLocator; }
            set { this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string FlowType
        { get { return this.flowType; } set { this.flowType = value; } }

        public string CheckinSessionKey
        { get { return this.checkinSessionKey; } set { this.checkinSessionKey = value; } }

        public string ReviewChangeBackBtnText
        { get { return this.reviewChangeBackBtnText; } set { this.reviewChangeBackBtnText = value; } }

        public string AncillaryRefundFormOfPayment { get { return this.ancillaryRefundFormOfPayment; } set { this.ancillaryRefundFormOfPayment = value; } }

        public string LastName
        {
            get { return this.lastName; }
            set { this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string FsrTitle
        {
            get { return this.fsrTitle; }
            set { this.fsrTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string FsrChangeFeeTxt
        {
            get
            {
                return this.fsrChangeFeeTxt;
            }
            set
            {
                this.fsrChangeFeeTxt = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string refundFormOfPaymentMessage;
        public string RefundFormOfPaymentMessage
        {
            get
            {
                return this.refundFormOfPaymentMessage;
            }
            set
            {
                this.refundFormOfPaymentMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string changeTripTitle;
        public string ChangeTripTitle
        {
            get
            {
                return this.changeTripTitle;
            }
            set
            {
                this.changeTripTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string feeWaiverMessage;
        public string FeeWaiverMessage
        {
            get
            {
                return this.feeWaiverMessage;
            }
            set
            {
                this.feeWaiverMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        

        private bool isLastTripFSR = false;

        public bool IsLastTripFSR
        {
            get { return isLastTripFSR; }
            set { isLastTripFSR = value; }
        }

        private bool isRefundBillingAddressRequired = false;
        public bool IsRefundBillingAddressRequired
        {
            get { return isRefundBillingAddressRequired; }
            set { isRefundBillingAddressRequired = value; }
        }

        private MOBAddress refundAddress;
        public MOBAddress RefundAddress
        {
            get { return refundAddress; }
            set { refundAddress = value; }
        }

        private MOBAddress ffcrAddress;
        public MOBAddress FFCRAddress
        {
            get { return ffcrAddress; }
            set { ffcrAddress = value; }
        }

        private string changeFlightHeaderText;
        public string ChangeFlightHeaderText
        {
            get { return changeFlightHeaderText; }
            set { changeFlightHeaderText = value; }
        }

        private string flightHeaderText;
        public string FlightHeaderText
        {
            get { return flightHeaderText; }
            set { flightHeaderText = value; }
        }

        private bool isUsedPNR;
        public bool IsUsedPNR
        {
            get { return isUsedPNR; }
            set { isUsedPNR = value; }
        }

        private bool isReshopWithFutureFlightCredit;
        public bool IsReshopWithFutureFlightCredit
        {
            get { return isReshopWithFutureFlightCredit; }
            set { isReshopWithFutureFlightCredit = value; }
        }
        private bool displayNonResidualCreditMessage;
        public bool DisplayNonResidualCreditMessage
        {
            get { return displayNonResidualCreditMessage; }
            set { displayNonResidualCreditMessage = value; }
        }

    }
}
