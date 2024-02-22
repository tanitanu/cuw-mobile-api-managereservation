using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class ServicesAndFees
    {
        private string agentDutyCode = string.Empty;
        public string AgentDutyCode
        {
            get
            {
                return this.agentDutyCode;
            }
            set
            {
                this.agentDutyCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        private string agentId = string.Empty;
        public string AgentId
        {
            get
            {
                return this.agentId;
            }
            set
            {
                this.agentId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        private string agentTripleA = string.Empty;
        public string AgentTripleA
        {
            get
            {
                return this.agentTripleA;
            }
            set
            {
                this.agentTripleA = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        private bool available;
        public bool Available
        {
            get
            {
                return this.available;
            }
            set
            {
                this.available = value;
            }
        }

        private decimal baseFee;
        public decimal BaseFee
        {
            get
            {
                return this.baseFee;
            }
            set
            {
                this.baseFee = value;
            }
        }

        private string currency = string.Empty;
        public string Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        private string eliteStatus = string.Empty;
        public string EliteStatus
        {
            get
            {
                return this.eliteStatus;
            }
            set
            {
                this.eliteStatus = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string feeWaiveType = string.Empty;
        public string FeeWaiveType
        {
            get
            {
                return this.feeWaiveType;
            }
            set
            {
                this.feeWaiveType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private bool isDefault;
        public bool IsDefault
        {
            get
            {
                return this.isDefault;
            }
            set
            {
                this.isDefault = value;
            }
        }

        private bool isFeeOverriden;
        public bool IsFeeOverriden
        {
            get
            {
                return this.isFeeOverriden;
            }
            set
            {
                this.isFeeOverriden = value;
            }
        }

        private string overrideReason = string.Empty;
        public string OverrideReason
        {
            get
            {
                return this.overrideReason;
            }
            set
            {
                this.overrideReason = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string program = string.Empty;
        public string Program
        {
            get
            {
                return this.program;
            }
            set
            {
                this.program = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        private string seatFeature = string.Empty;
        public string SeatFeature
        {
            get
            {
                return this.seatFeature;
            }
            set
            {
                this.seatFeature = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        private string seatNumber = string.Empty;
        public string SeatNumber
        {
            get
            {
                return this.seatNumber;
            }
            set
            {
                this.seatNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private decimal tax;
        public decimal Tax
        {
            get
            {
                return this.tax;
            }
            set
            {
                this.tax = value;
            }
        }

        private decimal totalFee;
        public decimal TotalFee
        {
            get
            {
                return this.totalFee;
            }
            set
            {
                this.totalFee = value;
            }
        }
        private Int32 totalMiles;
        public Int32 TotalMiles
        {
            get
            {
                return this.totalMiles;
            }
            set
            {
                this.totalMiles = value;
            }
        }
        private string displayTotalMiles;
        public string DisplayTotalMiles
        {
            get
            {
                return this.displayTotalMiles;
            }
            set
            {
                this.displayTotalMiles = value;
            }
        }

        private Int32 milesFee;
        public Int32 MilesFee
        {
            get
            {
                return this.milesFee;
            }
            set
            {
                this.milesFee = value;
            }
        }
        private string displayMilesFee;
        public string DisplayMilesFee
        {
            get
            {
                return this.displayMilesFee;
            }
            set
            {
                this.displayMilesFee = value;
            }
        }

        private string displayFeesWithMiles;
        public string DisplayFeesWithMiles
        {
            get
            {
                return this.displayFeesWithMiles;
            }
            set
            {
                this.displayFeesWithMiles = value;
            }
        }

        private string originalPrice;
        private string couponCode;
        public string OriginalPrice
        {
            get
            {
                return this.originalPrice;
            }
            set
            {
                this.originalPrice = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CouponCode
        {
            get
            {
                return this.couponCode;
            }
            set
            {
                this.couponCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
