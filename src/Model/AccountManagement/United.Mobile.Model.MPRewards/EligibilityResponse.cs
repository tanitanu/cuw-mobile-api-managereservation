using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using MOBPNRSegment = United.Mobile.Model.Common.MOBPNRSegment;

namespace United.Mobile.Model.MPRewards
{
    [Serializable()]
    public class EligibilityResponse
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Reshopping.EligibilityResponse";
        public string ObjectName
        {
            get
            {
                return this.objectName;
            }
            set
            {
                this.objectName = value;
            }
        }
        #endregion

        private bool isElf;
        private bool isIBELite;
        private bool isIBE;
        private bool isCBE;
        private List<MOBPNRPassenger> passengers;
        private List<MOBPNRSegment> segments;
        private List<MOBPNRPassenger> infantInLaps;
        private bool isBEChangeEligible;
        private bool isUnaccompaniedMinor;
        private bool isReshopWithFutureFlightCredit;
        private bool isCorporateBooking;
        private string corporateVendorName;
        private bool isCheckinEligible;
        private bool isAgencyBooking;
        private string agencyName;
        private Boolean hasScheduleChange;
        private Boolean isSCChangeEligible;
        private Boolean isSCRefundEligible;
        private Boolean isATREEligible;
        private Boolean isChangeEligible;
        private Boolean isMilesAndMoney;
        private Boolean is24HrFlexibleBookingPolicy;
        private Boolean isJSENonChangeableFare;

        public Boolean Is24HrFlexibleBookingPolicy { get { return this.is24HrFlexibleBookingPolicy; } set { this.is24HrFlexibleBookingPolicy = value; } }
        public Boolean IsJSENonChangeableFare { get { return this.isJSENonChangeableFare; } set { this.isJSENonChangeableFare = value; } }

        public Boolean IsATREEligible { get { return this.isATREEligible; } set { this.isATREEligible = value; } }
        public Boolean IsChangeEligible { get { return this.isChangeEligible; } set { this.isChangeEligible = value; } }
        public Boolean IsMilesAndMoney { get { return this.isMilesAndMoney; } set { this.isMilesAndMoney = value; } }
        public Boolean IsSCChangeEligible { get { return this.isSCChangeEligible; } set { this.isSCChangeEligible = value; } }
        public Boolean IsSCRefundEligible { get { return this.isSCRefundEligible; } set { this.isSCRefundEligible = value; } }
        public Boolean IsBEChangeEligible { get { return this.isBEChangeEligible; } set { this.isBEChangeEligible = value; } }
        public Boolean IsCorporateBooking { get { return this.isCorporateBooking; } set { this.isCorporateBooking = value; } }
        public string CorporateVendorName { get { return this.corporateVendorName; } set { this.corporateVendorName = value; } }
        public Boolean IsCheckinEligible { get { return this.isCheckinEligible; } set { this.isCheckinEligible = value; } }
        public Boolean IsAgencyBooking { get { return this.isAgencyBooking; } set { this.isAgencyBooking = value; } }
        public string AgencyName { get { return this.agencyName; } set { this.agencyName = value; } }
        public Boolean HasScheduleChange { get { return this.hasScheduleChange; } set { this.hasScheduleChange = value; } }


        public bool IsElf
        {
            get { return this.isElf; }
            set { this.isElf = value; }
        }
        public bool IsReshopWithFutureFlightCredit
        {
            get { return this.isReshopWithFutureFlightCredit; }
            set { this.isReshopWithFutureFlightCredit = value; }
        }

        public bool IsUnaccompaniedMinor
        {
            get { return this.isUnaccompaniedMinor; }
            set { this.isUnaccompaniedMinor = value; }
        }

        public List<MOBPNRSegment> Segments
        {
            get
            {
                return this.segments;
            }
            set
            {
                this.segments = value;
            }
        }

        public bool IsIBELite
        {
            get { return isIBELite; }
            set { isIBELite = value; }
        }

        public bool IsIBE
        {
            get { return isIBE; }
            set { isIBE = value; }
        }

        public bool IsCBE
        {
            get { return isCBE; }
            set { isCBE = value; }
        }

        public List<MOBPNRPassenger> Passengers
        {
            get
            {
                return this.passengers;
            }
            set
            {
                this.passengers = value;
            }
        }
        public List<MOBPNRPassenger> InfantInLaps
        {
            get { return this.infantInLaps; }
            set { this.infantInLaps = value; }
        }
    }
}
