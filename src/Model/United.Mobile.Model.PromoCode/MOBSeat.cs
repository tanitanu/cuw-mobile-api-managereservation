using System;
using System.Collections.Generic;
using System.Text;
using United.Service.Presentation.PriceModel;

namespace United.Mobile.Model.PromoCode
{
    [Serializable()]
    public  class MOBSeat
    {
        private string seatAssignment;
        private string oldSeatAssignment;
        private string attribute;
        private string origin;
        private string destination;
        private string flightNumber;
        private string departureDate;
        private string travelerSharesIndex;
        private int key;
        private bool uaOperated;
        private decimal price;
        private decimal priceAfterTravelerCompanionRules;
        private string currency;
        private string programCode;
        private string seatType;
        private decimal oldSeatPrice;
        private string oldSeatCurrency;
        private string oldSeatProgramCode;
        private string oldSeatType;
        private string oldSeatEDDTransactionId;
        private bool isEPA;
        private bool isEPAFreeCompanion;
        private string seatFeature;
        private bool limitedRecline;
        private string pcuOfferOptionId;

        private bool isCouponApplied;
        private decimal priceAfterCouponApplied;
        private string segmentIndex;
        private List<PriceAdjustment> adjustments;
        private decimal priceBeforeCouponApplied;

        private Int32 miles;
        private Int32 milesAfterTravelerCompanionRules;
        private Int32 oldSeatMiles;
        private Int32 milesBeforeCouponApplied;
        private Int32 milesAfterCouponApplied;
        private string displayMiles;
        private string displayMilesAfterTravelerCompanionRules;
        private string displayOldSeatMiles;
        private string displayMilesBeforeCouponApplied;
        private string displayMilesAfterCouponApplied;
        public decimal PriceBeforeCouponApplied
        {
            get { return priceBeforeCouponApplied; }
            set { priceBeforeCouponApplied = value; }
        }

        public List<PriceAdjustment> Adjustments
        {
            get { return adjustments; }
            set { adjustments = value; }
        }

        public string SegmentIndex
        {
            get { return segmentIndex; }
            set { segmentIndex = value; }
        }

        public decimal PriceAfterCouponApplied
        {
            get { return priceAfterCouponApplied; }
            set { priceAfterCouponApplied = value; }
        }

        public bool IsCouponApplied
        {
            get { return isCouponApplied; }
            set { isCouponApplied = value; }
        }

        public bool LimitedRecline
        {
            get { return limitedRecline; }
            set { limitedRecline = value; }
        }

        public string SeatAssignment
        {
            get
            {
                return this.seatAssignment;
            }
            set
            {
                this.seatAssignment = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string OldSeatAssignment
        {
            get
            {
                return this.oldSeatAssignment;
            }
            set
            {
                this.oldSeatAssignment = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Attribute
        {
            get
            {
                return this.attribute;
            }
            set
            {
                this.attribute = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Origin
        {
            get { return this.origin; }
            set { this.origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string Destination
        {
            get { return this.destination; }
            set { this.destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string FlightNumber
        {
            get { return this.flightNumber; }
            set { this.flightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string DepartureDate
        {
            get { return this.departureDate; }
            set { this.departureDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string TravelerSharesIndex
        {
            get { return this.travelerSharesIndex; }
            set { this.travelerSharesIndex = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public int Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        public bool UAOperated
        {
            get { return this.uaOperated; }
            set { this.uaOperated = value; }
        }

        public decimal Price
        {
            get { return this.price; }
            set { this.price = value; }
        }

        public decimal PriceAfterTravelerCompanionRules
        {
            get { return this.priceAfterTravelerCompanionRules; }
            set { this.priceAfterTravelerCompanionRules = value; }
        }

        public string Currency
        {
            get { return this.currency; }
            set { this.currency = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string ProgramCode
        {
            get { return this.programCode; }
            set { this.programCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string SeatType
        {
            get { return this.seatType; }
            set { this.seatType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public decimal OldSeatPrice
        {
            get { return this.oldSeatPrice; }
            set { this.oldSeatPrice = value; }
        }

        public string OldSeatCurrency
        {
            get { return this.oldSeatCurrency; }
            set { this.oldSeatCurrency = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string OldSeatProgramCode
        {
            get { return this.oldSeatProgramCode; }
            set { this.oldSeatProgramCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string OldSeatType
        {
            get { return this.oldSeatType; }
            set { this.oldSeatType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string OldSeatEDDTransactionId
        {
            get { return this.oldSeatEDDTransactionId; }
            set { this.oldSeatEDDTransactionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public bool IsEPA
        {
            get { return this.isEPA; }
            set { this.isEPA = value; }
        }

        public bool IsEPAFreeCompanion
        {
            get { return this.isEPAFreeCompanion; }
            set { this.isEPAFreeCompanion = value; }
        }

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

        public string PcuOfferOptionId
        {
            get { return pcuOfferOptionId; }
            set { pcuOfferOptionId = value; }
        }

        public Int32 Miles
        {
            get { return this.miles; }
            set { this.miles = value; }
        }
        public Int32 MilesAfterTravelerCompanionRules
        {
            get { return this.milesAfterTravelerCompanionRules; }
            set { this.milesAfterTravelerCompanionRules = value; }
        }
        public Int32 OldSeatMiles
        {
            get { return this.oldSeatMiles; }
            set { this.oldSeatMiles = value; }
        }
        public Int32 MilesBeforeCouponApplied
        {
            get { return this.milesBeforeCouponApplied; }
            set { this.milesBeforeCouponApplied = value; }
        }
        public Int32 MilesAfterCouponApplied
        {
            get { return this.milesAfterCouponApplied; }
            set { this.milesAfterCouponApplied = value; }
        }
        public string DisplayMiles
        {
            get { return this.displayMiles; }
            set { this.displayMiles = value; }
        }
        public string DisplayMilesAfterTravelerCompanionRules
        {
            get { return this.displayMilesAfterTravelerCompanionRules; }
            set { this.displayMilesAfterTravelerCompanionRules = value; }
        }
        public string DisplayOldSeatMiles
        {
            get { return this.displayOldSeatMiles; }
            set { this.displayOldSeatMiles = value; }
        }
        public string DisplayMilesBeforeCouponApplied
        {
            get { return this.displayMilesBeforeCouponApplied; }
            set { this.displayMilesBeforeCouponApplied = value; }
        }
        public string DisplayMilesAfterCouponApplied
        {
            get { return this.displayMilesAfterCouponApplied; }
            set { this.displayMilesAfterCouponApplied = value; }
        }


    }
}

