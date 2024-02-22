using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace United.Mobile.Model.SeatMapEngine
{
    [Serializable]
    public class MOBSHOPShoppingProduct
    {
        private string productId = string.Empty;
        public string ProductId
        {
            get { return productId; }
            set { productId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string type = string.Empty;
        public string Type
        {
            get { return type; }
            set { type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }


        private string subType = string.Empty;
        public string SubType
        {
            get { return subType; }
            set { subType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string longCabin = string.Empty;
        public string LongCabin
        {
            get { return longCabin; }
            set
            {
                longCabin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();

                // For backwards compatibility with releases before 17B
                // remove logic after force-upgrading all 17A and earlier clients
                if (string.IsNullOrEmpty(cabin))
                {
                    cabin = longCabin;
                }
            }
        }
        private string reshopFees = string.Empty;
        public string ReshopFees
        {
            get { return reshopFees; }
            set { reshopFees = value; }
        }

        private string cabin = string.Empty;
        public string Cabin
        {
            get { return cabin; }
            set { cabin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string description = string.Empty;
        public string Description
        {
            get { return description; }
            set { description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string price = string.Empty;
        public string Price
        {
            get { return price; }
            set { price = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string milesDisplayValue = string.Empty;
        public string MilesDisplayValue
        {
            get { return milesDisplayValue; }
            set { milesDisplayValue = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private decimal priceAmount = 0;
        public decimal PriceAmount
        {
            get { return priceAmount; }
            set { priceAmount = value; }
        }

        private decimal milesDisplayAmount = 0;
        public decimal MilesDisplayAmount
        {
            get { return milesDisplayAmount; }
            set { milesDisplayAmount = value; }
        }

        private string meal = string.Empty;
        public string Meal
        {
            get { return meal; }
            set { meal = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private MOBSHOPShoppingProductDetail productDetail;
        public MOBSHOPShoppingProductDetail ProductDetail
        {
            get { return productDetail; }
            set { productDetail = value; }
        }

        private bool isMixedCabin = false;
        public bool IsMixedCabin
        {
            get { return isMixedCabin; }
            set { isMixedCabin = value; }
        }

        private List<string> mixedCabinSegmentMessages;
        public List<string> MixedCabinSegmentMessages
        {
            get { return mixedCabinSegmentMessages; }
            set { mixedCabinSegmentMessages = value; }
        }

        private string awardType = string.Empty;
        public string AwardType
        {
            get { return awardType; }
            set { awardType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string allCabinButtonText = string.Empty;
        public string AllCabinButtonText
        {
            get { return allCabinButtonText; }
            set { allCabinButtonText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private bool isSelectedCabin = false;
        public bool IsSelectedCabin
        {
            get { return isSelectedCabin; }
            set { isSelectedCabin = value; }
        }

        private int mileageButton = -1;
        public int MileageButton
        {
            get { return mileageButton; }
            set { mileageButton = value; }
        }

        private int seatsRemaining = 0;//value has been updated from -1 to 0 based on Varsheny Shashank and Padmanabhan Praveena's analysis
        public int SeatsRemaining
        {
            get { return seatsRemaining; }
            set { seatsRemaining = value; }
        }

        private string pqdText = string.Empty;
        public string PqdText
        {
            get { return pqdText; }
            set { pqdText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string pqmText = string.Empty;
        public string PqmText
        {
            get { return pqmText; }
            set { pqmText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string rdmText = string.Empty;
        public string RdmText
        {
            get { return rdmText; }
            set { rdmText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private bool isPremierCabinSaver = false;
        public bool ISPremierCabinSaver
        {
            get { return isPremierCabinSaver; }
            set { isPremierCabinSaver = value; }
        }

        private bool isUADiscount = false;
        public bool IsUADiscount
        {
            get { return isUADiscount; }
            set { isUADiscount = value; }
        }

        private bool isELF = false;
        public bool IsELF
        {
            get { return isELF; }
            set { isELF = value; }
        }

        private string cabinType = string.Empty;
        public string CabinType
        {
            get { return cabinType; }
            set { cabinType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
        private string priceFromText = string.Empty;
        public string PriceFromText
        {
            get { return priceFromText; }
            set { priceFromText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private bool isIBELite;
        public bool IsIBELite
        {
            get { return isIBELite; }
            set { isIBELite = value; }
        }

        private bool isIBE;
        public bool IsIBE
        {
            get { return isIBE; }
            set { isIBE = value; }
        }

        private string shortProductName;

        /// <summary>
        /// Currently, only populated for IBELite product
        /// </summary>
        public string ShortProductName
        {
            get { return shortProductName; }
            set { shortProductName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string productCode;

        public string ProductCode
        {
            get { return productCode; }
            set { productCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private List<MOBStyledText> productBadges;
        public List<MOBStyledText> ProductBadges
        {
            get { return productBadges; }
            set { productBadges = value; }
        }

        private string fareContentDescription;

        public string FareContentDescription
        {
            get { return fareContentDescription; }
            set { fareContentDescription = value; }
        }

        private string columnID;

        public string ColumnID
        {
            get { return columnID; }
            set { columnID = value; }
        }

        private string priceApplyLabelText;

        public string PriceApplyLabelText
        {
            get { return priceApplyLabelText; }
            set { priceApplyLabelText = value; }
        }
        private bool isReshopCredit;
        public bool IsReshopCredit
        {
            get { return isReshopCredit; }
            set { isReshopCredit = value; }
        }

        private CreditTypeColor reshopCreditColor;
        public CreditTypeColor ReshopCreditColor
        {
            get { return reshopCreditColor; }
            set { reshopCreditColor = value; }
        }

        private string cabinDescription;
        public string CabinDescription
        {
            get { return cabinDescription; }
            set { cabinDescription = value; }
        }
        private string bookingCode;
        public string BookingCode
        {
            get { return bookingCode; }
            set { bookingCode = value; }
        }

        private string strikeThroughDisplayValue;

        public string StrikeThroughDisplayValue
        {
            get { return strikeThroughDisplayValue; }
            set { strikeThroughDisplayValue = value; }
        }
        private string interimScreenCode;

        public string InterimScreenCode
        {
            get { return interimScreenCode; }
            set { interimScreenCode = value; }
        }
    }

    [Serializable]
    public enum CreditTypeColor
    {
        [EnumMember(Value = "NONE")] //DEFAULT
        NONE,
        [EnumMember(Value = "GREEN")]
        GREEN,
    }
}
