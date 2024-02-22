using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MakeReservationResponse : MOBResponse
    {
        private string sessionID = string.Empty;
        private MOBSHOPReservation reservation;
        private List<string> dotBagRules;
        private DOTBaggageInfo dotBaggageInfo;
        private string shareFlightTitle = string.Empty;
        private string shareFlightMessage = string.Empty;
        private string warning = string.Empty;
        private string fqtvNameMismatchMessage = string.Empty;
        private List<string> disclaimer;
        private MOBSHOPFormOfPayment formOfPayment;
        private List<ClubDayPass> passes;
        private ClubPassPurchaseRequest clubPassPurchaseRequest;
        private string otpMessage = string.Empty;
        private MOBCreditCard formOfPaymentForTPI;
        private string confirmationMsgForTPI;
        private string specialNeedsMessage;
        private MOBSHOPResponseStatusItem responseStatusItem;

        public string SessionID
        {
            get
            {
                return this.sessionID;
            }
            set
            {
                this.sessionID = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBSHOPReservation Reservation
        {
            get
            {
                return this.reservation;
            }
            set
            {
                this.reservation = value;
            }
        }

        public List<string> DOTBagRules
        {
            get
            {
                return this.dotBagRules;
            }
            set
            {
                this.dotBagRules = value;
            }
        }

        public DOTBaggageInfo DOTBaggageInfo
        {
            get
            {
                return this.dotBaggageInfo;
            }
            set
            {
                this.dotBaggageInfo = value;
            }
        }

        public string ShareFlightTitle
        {
            get
            {
                return this.shareFlightTitle;
            }
            set
            {
                this.shareFlightTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ShareFlightMessage
        {
            get
            {
                return this.shareFlightMessage;
            }
            set
            {
                this.shareFlightMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Warning
        {
            get
            {
                return this.warning;
            }
            set
            {
                this.warning = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FQTVNameMismatchMessage
        {
            get
            {
                return this.fqtvNameMismatchMessage;
            }
            set
            {
                this.fqtvNameMismatchMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<string> Disclaimer
        {
            get
            {
                return this.disclaimer;
            }
            set
            {
                this.disclaimer = value;
            }
        }

        public MOBSHOPFormOfPayment FormOfPayment
        {
            get
            {
                return this.formOfPayment;
            }
            set
            {
                this.formOfPayment = value;
            }
        }

        public List<ClubDayPass> Passes
        {
            get
            {
                return this.passes;
            }
            set
            {
                this.passes = value;
            }
        }

        public ClubPassPurchaseRequest ClubPassPurchaseRequest
        {
            get
            {
                return this.clubPassPurchaseRequest;
            }
            set
            {
                this.clubPassPurchaseRequest = value;
            }
        }

        public string OTPMessage
        {
            get
            {
                return this.otpMessage;
            }
            set
            {
                this.otpMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBSHOPResponseStatusItem ResponseStatusItem
        {
            get { return responseStatusItem; }
            set { responseStatusItem = value; }
        }

        public MOBCreditCard FormOfPaymentForTPI
        {
            get
            {
                return this.formOfPaymentForTPI;
            }
            set
            {
                this.formOfPaymentForTPI = value;
            }
        }

        public string ConfirmationMsgForTPI
        {
            get
            {
                return this.confirmationMsgForTPI;
            }
            set
            {
                this.confirmationMsgForTPI = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string SpecialNeedsMessage
        {
            get { return specialNeedsMessage; }
            set { specialNeedsMessage = value; }
        }
    }

    [Serializable]
    public class MOBSHOPResponseStatusItem
    {
        private MOBSHOPResponseStatus status;
        public MOBSHOPResponseStatus Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        private List<MOBItem> statusMessages;
        public List<MOBItem> StatusMessages
        {
            get { return statusMessages; }
            set { statusMessages = value; }
        }
    }

    [Serializable]
    public enum ResponseStatus
    {
        [EnumMember(Value = "1")]
        ReshopUnableToComplete,
        [EnumMember(Value = "2")]
        ReshopChangePending,
        [EnumMember(Value = "3")]
        ReshopBENonElgible,
        [EnumMember(Value = "4")]
        ReshopUnableToChange,
        [EnumMember(Value = "5")]
        PcuUpgradeFailed,
        [EnumMember(Value = "6")]
        FailedToGetBagChargeInfo,
        [EnumMember(Value = "7")]
        ReshopAgencyCheckinEligible,
        [EnumMember(Value = "8")]
        ReshopCheckinEligible,
        [EnumMember(Value = "9")]
        ReshopChangeOfferBEBuyOut,
        [EnumMember(Value = "10")]
        ReshopOTFShopEligible,
    }
}
