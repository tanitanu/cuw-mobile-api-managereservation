using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;
using CreditTypeColor = United.Mobile.Model.Shopping.CreditTypeColor;

namespace United.Persist.Definition.Shopping
{
    public class MOBSHOPShoppingProduct
    {
        private string productId = string.Empty;
        public string ProductId
        {
            get { return productId; }
            set { productId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
        public string Type { get; set; } = string.Empty;    
        public string SubType { get; set; } = string.Empty;        
        public string LongCabin { get; set; } = string.Empty;
        public string ReshopFees { get; set; } = string.Empty;
        public string isReshopCredit { get; set; } = string.Empty;
        public CreditTypeColor reshopCreditColor { get; set; } 
        public string Cabin { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string MilesDisplayValue { get; set; } = string.Empty;
        public decimal PriceAmount { get; set; }
        public decimal MilesDisplayAmount { get; set; } 
        public string Meal { get; set; } = string.Empty;       
        public MOBSHOPShoppingProductDetail ProductDetail { get; set; }
        public bool IsMixedCabin { get; set; }
        public List<string> MixedCabinSegmentMessages { get; set; }
        public string AwardType { get; set; } = string.Empty;
        public string AllCabinButtonText { get; set; } = string.Empty;
        public bool IsSelectedCabin { get; set; }
        public int MileageButton { get; set; } = -1;
        public int SeatsRemaining { get; set; } = -1;
        public string PqdText { get; set; } = string.Empty;
        public string PqmText { get; set; } = string.Empty;
        public string RdmText { get; set; } = string.Empty;
        public bool ISPremierCabinSaver { get; set; }
        public bool IsUADiscount { get; set; }
        public bool IsELF { get; set; }
        public string CabinType { get; set; } = string.Empty;    
        public string PriceFromText { get; set; } = string.Empty;
        public bool IsIBELite { get; set; }        
        public bool IsIBE { get; set; }
        public string ShortProductName { get; set; }
        public string ProductCode { get; set; }
        public List<MOBStyledText> ProductBadges { get; set; }
        public string FareContentDescription { get; set; }
        public string ColumnID { get; set; }
        public string PriceApplyLabelText { get; set; }
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

}
