using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping.FormofPayment;
using United.Mobile.Model.Shopping.UpgradeCabin;
using United.Service.Presentation.CommonEnumModel;
using United.Utility.Enum;
using Newtonsoft.Json;


namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBShoppingCart
    {
        [JsonIgnore()]
        public string ObjectName { get; set; } = "United.Definition.MOBShoppingCart";
        public bool IsHidePaymentMethod { get; set; }

        public MOBSHOPInflightContactlessPaymentEligibility InFlightContactlessPaymentEligibility { get; set; }

        public MOBPromoCodeDetails PromoCodeDetails { get; set; }

        public List<MOBMobileCMSContentMessages> ConfirmationPageAlertMessages { get; set; } = new List<MOBMobileCMSContentMessages>();

        public TripShare TripShare { get; set; }

        public List<MOBFOPCertificate> ProfileTravelerCertificates { get; set; } = new List<MOBFOPCertificate>();

        public List<MOBFOPCertificateTraveler> CertificateTravelers { get; set; }

        public string BundleCartId { get; set; }

        public string PartialCouponEligibleUrl { get; set; }

        public string PartialCouponEligibleMessage { get; set; }

        public bool IsCouponEligibleProduct { get; set; }

        public MOBCPTraveler Travelers { get; set; }

        public bool IsCouponApplied { get; set; }

        public bool DisableCouponEditOption { get; set; }

        public string CouponCode { get; set; }

        public string CouponOfferDescription { get; set; }

        public string CartId { get; set; } = string.Empty;

        public string Flow { get; set; } = string.Empty;

        [XmlArrayItem("MOBProdDetail")]
        public List<ProdDetail> Products { get; set; }

        public List<UpgradeOption> UpgradeCabinProducts { get; set; } = new List<UpgradeOption>();

        public string TotalPoints { get; set; } = string.Empty;

        public string DisplayTotalPoints { get; set; } = string.Empty;

        public string TotalPrice { get; set; } = string.Empty;

        private string displayTotalPrice  = string.Empty;
        public string DisplayTotalPrice
        {
            get { return displayTotalPrice; }
            set { displayTotalPrice = value; }
        }

        public string DisplaySubTotalPrice { get; set; } = string.Empty;

        public string DisplayTaxesAndFees { get; set; } = string.Empty;

        public string TotalMiles { get; set; }

        public MOBFormofPaymentDetails FormofPaymentDetails { get; set; }

        public List<MOBCPTraveler> SCTravelers { get; set; }

        private string pointofSale = string.Empty;

        public string PointofSale
        {
            get { return pointofSale; }
            set { pointofSale = value; }
        }
        public string CurrencyCode { get; set; } = string.Empty;

        public List<Section> AlertMessages { get; set; } = new List<Section>();

        private List<MOBMobileCMSContentMessages> termsAndConditions;

        public List<MOBMobileCMSContentMessages> TermsAndConditions
        {
            get { return termsAndConditions; }
            set
            {
                termsAndConditions = value;
                PopulateTermsAndConditionsForOldClient(termsAndConditions);
            }
        }
        private string paymentTarget;
        public string PaymentTarget
        {
            get { return paymentTarget; }
            set { paymentTarget = value; }
        }

        private void PopulateTermsAndConditionsForOldClient(List<MOBMobileCMSContentMessages> termsAndConditions)
        {
            if (termsAndConditions != null && termsAndConditions.Any() &&
                Products != null && Products.Any() &&
                Products.FirstOrDefault() != null)
            {
                Products.FirstOrDefault().TermsAndCondition = termsAndConditions.FirstOrDefault();
            }
        }
        public List<MOBSHOPTrip> Trips { get; set; } = new List<MOBSHOPTrip>();

        public List<MOBSHOPPrice> Prices { get; set; }

        public List<List<MOBSHOPTax>> Taxes { get; set; } = new List<List<MOBSHOPTax>>();

        public List<MOBItem> Captions { get; set; }

        public List<MOBItem> ELFLimitations { get; set; } = new List<MOBItem>();

        public string DisplayTotalMiles { get; set; }

        public string FlightShareMessage { get; set; } = string.Empty;

        public List<Section> PaymentAlerts { get; set; } = new List<Section>();

        public List<Section> DisplayMessage { get; set; }

        public bool IsMultipleTravelerEtcFeatureClientToggleEnabled { get; set; }

        //[XmlArray("MOBULTripInfo")]
        public ULTripInfo TripInfoForUplift { get; set; }
        private int cslWorkFlowType;

        public int CslWorkFlowType
        {
            get { return cslWorkFlowType; }
            set { cslWorkFlowType = value; }
        }
        #region HandleCouponWhenFopIsUplift
        private void HandleCouponWhenFopIsUplift()
        {
            if (this.Flow != FlowType.VIEWRES.ToString())
                return;

            UpdateIsCouponEligibleProduct();
            UpdateDisableCouponEditOption();
        }

        private void UpdateDisableCouponEditOption()
        {
            // this.DisableCouponEditOption = this.FormofPaymentDetails?.FormOfPaymentType?.ToUpper() == FormofPayment.Uplift.ToString().ToUpper() && !string.IsNullOrEmpty(this.CouponCode);
        }

        private void UpdateIsCouponEligibleProduct()
        {

            //this.IsCouponEligibleProduct = this.FormofPaymentDetails?.FormOfPaymentType?.ToUpper() == FormofPayment.Uplift.ToString().ToUpper() && string.IsNullOrEmpty(this.CouponCode)
            //? false
            //: this.Products?.Any(p => ConfigurationManager.AppSettings["IsCouponEligibleProduct"]?.Split('|')?.ToList().Contains(p?.Code) ?? false) ?? false;
        }
        #endregion

        public Cart OmniCart { get; set; }

        public MOBShoppingCart()
        {
            Products = new List<ProdDetail>();
            TermsAndConditions = new List<MOBMobileCMSContentMessages>();
            Prices = new List<MOBSHOPPrice>();
            Captions = new List<MOBItem>();
            DisplayMessage = new List<Section>();
            CertificateTravelers = new List<MOBFOPCertificateTraveler>();
            SCTravelers = new List<MOBCPTraveler>();
        }
    }
    [Serializable()]
    public class ProdDetail
    {
        public List<CouponDetails> CouponDetails { get; set; } 

        public string ProdDescription { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string ProdTotalPrice { get; set; } = string.Empty;

        private string prodDisplayTotalPrice = string.Empty;
        public string ProdDisplayTotalPrice
        {
            get { return prodDisplayTotalPrice; }
            set { prodDisplayTotalPrice = value; }
        }
        public string ProdOtherPrice { get; set; } = string.Empty;

        public string ProdDisplayOtherPrice { get; set; } = string.Empty;

        public string ProdDisplaySubTotal { get; set; } = string.Empty;

        public string ProdDisplayTaxesAndFees { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "termAndCondition")]
        public MOBMobileCMSContentMessages TermsAndCondition { get; set; }

        [XmlArrayItem("MOBProductSegmentDetail")]
        public List<ProductSegmentDetail> Segments { get; set; }

        public Int32 ProdTotalMiles { get; set; }
        private string prodDisplayTotalMiles;
        public string ProdDisplayTotalMiles
        {
            get { return prodDisplayTotalMiles; }
            set { prodDisplayTotalMiles = value; }
        }

        public Int32 ProdTotalPoints { get; set; }

        public string ProdDisplayTotalPoints { get; set; }
        public List<MOBTypeOption> LineItems { get; set; }
        public string ProdOriginalPrice { get; set; }
        public ProdDetail()
        {
            CouponDetails = new List<CouponDetails>();
            LineItems = new List<MOBTypeOption>();
            Segments = new List<ProductSegmentDetail>();
        }

    }

    [Serializable()]
    public class ProductSegmentDetail
    {
        public string SegmentInfo { get; set; }

        public string ProductId { get; set; } = string.Empty;

        public string TripId { get; set; } = string.Empty;

        public string SegmentId { get; set; } = string.Empty;
        public List<string> ProductIds { get; set; }
        
        [XmlArrayItem("MOBProductSubSegmentDetail")]
        public List<ProductSubSegmentDetail> SubSegmentDetails { get; set; }
        public ProductSegmentDetail()
        {
            SubSegmentDetails = new List<ProductSubSegmentDetail>();
        }
    }
    [Serializable()]
    public class ProductSubSegmentDetail
    {
        //Required for strike off price to identify the flight segments uniquely

        private string price = string.Empty;

        public string Passenger { get; set; } = string.Empty;

        public string Price
        {
            get { return price; }
            set { price = value; }
        }

        public string DisplayPrice { get; set; } = string.Empty;

        public string SegmentDescription { get; set; } = string.Empty;

        public bool IsPurchaseFailure { get; set; }

        public string StrikeOffPrice { get; set; } = string.Empty;

        public string DisplayStrikeOffPrice { get; set; } = string.Empty;

        public string SeatCode { get; set; } = string.Empty;

        public string FlightNumber { get; set; } = string.Empty;

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public Int32 Miles { get; set; }

        private string displayMiles;
        public string DisplayMiles
        {
            get { return displayMiles; }
            set { displayMiles = value; }
        }

        public Int32 StrikeOffMiles { get; set; }

        private string displayStrikeOffMiles  = string.Empty;
        public string DisplayStrikeOffMiles
        {
            get { return displayStrikeOffMiles; }
            set { displayStrikeOffMiles = value; }
        }

        private string orginalPrice;

        public string OrginalPrice
        {
            get { return orginalPrice; }
            set { orginalPrice = value; }
        }

        public MOBPromoCode PromoDetails { get; set; }

        public string ProductDescription { get; set; }

        public List<string> ProdDetailDescription { get; set; }

        private string displayOriginalPrice;

        public string DisplayOriginalPrice
        {
            get { return displayOriginalPrice; }
            set { displayOriginalPrice = value; }
        }

        public string SegmentInfo { get; set; }

        private List<MOBPaxDetails> paxDetails;

        public List<MOBPaxDetails> PaxDetails
        {
            get { return paxDetails; }
            set { paxDetails = value; }
        }


    }
    [Serializable()]
    public class SCProductContext
    {
        public string Description { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
    [Serializable()]
    public class CouponDetails
    {
        public string Description { get; set; } = string.Empty;

        public string Product { get; set; } = string.Empty;

        public string PromoCode { get; set; } = string.Empty;

        public string IsCouponEligible { get; set; } = string.Empty;

        public CouponDiscountType DiscountType { get; set; }

    }
    [Serializable()]
    public class Cart
    {
        public int CartItemsCount { get; set; }

        public MOBItem PayLaterPrice { get; set; }

        private string costBreakdownFareHeader = "Fare";

        public string CostBreakdownFareHeader
        {
            get { return costBreakdownFareHeader; }
            set { costBreakdownFareHeader = value; }
        }
        public MOBItem TotalPrice { get; set; }

        private string navigateToScreen;

        public string NavigateToScreen
        {
            get { return navigateToScreen; }
            set { navigateToScreen = value; }
        }
        private bool isCallRegisterOffers;

        public bool IsCallRegisterOffers
        {
            get { return isCallRegisterOffers; }
            set { isCallRegisterOffers = value; }
        }
         private List<MOBOmniCartRepricingInfo> omniCartPricingInfos;

        public List<MOBOmniCartRepricingInfo> OmniCartPricingInfos
        {
            get { return omniCartPricingInfos; }
            set { omniCartPricingInfos = value; }
        }
        private int navigateToSeatmapSegmentNumber;

        public int NavigateToSeatmapSegmentNumber
        {
            get { return navigateToSeatmapSegmentNumber; }
            set { navigateToSeatmapSegmentNumber = value; }
        }
        private bool isUpliftEligible;
        public bool IsUpliftEligible
        {
            get { return isUpliftEligible; }
            set { isUpliftEligible = value; }
        }

        private List<MOBSection> fopDetails;
        public List<MOBSection> FOPDetails
        {
            get { return fopDetails; }
            set { fopDetails = value; }
        }
        private MOBSection additionalMileDetail;
        public MOBSection AdditionalMileDetail
        {
            get { return additionalMileDetail; }
            set { additionalMileDetail = value; }
        }

        private string corporateDisclaimerText;

        public string CorporateDisclaimerText
        {
            get { return corporateDisclaimerText; }
            set { corporateDisclaimerText = value; }
        }
    }

    [Serializable()]
    public class MOBOmniCartRepricingInfo
    {
        private string product;
        public string Product
        {
            get { return product; }
            set { product = value; }
        }
        private MOBSection repriceAlertMessage;

        public MOBSection RepriceAlertMessage
        {
            get { return repriceAlertMessage; }
            set { repriceAlertMessage = value; }
        }
        private List<MOBOmniCartRepricingSegmentInfo> segments;

        public List<MOBOmniCartRepricingSegmentInfo> Segments
        {
            get { return segments; }
            set { segments = value; }
        }


    }

    [Serializable()]
    public class MOBOmniCartRepricingSegmentInfo
    {
        private int segmentNumber;

        public int SegmentNumber
        {
            get { return segmentNumber; }
            set { segmentNumber = value; }
        }

    }

    [Serializable()]
    public class MOBCart
    {
        private int cartItemsCount;

        public int CartItemsCount
        {
            get { return cartItemsCount; }
            set { cartItemsCount = value; }
        }
        private MOBItem payLaterPrice;

        public MOBItem PayLaterPrice
        {
            get { return payLaterPrice; }
            set { payLaterPrice = value; }
        }

        private string costBreakdownFareHeader = "Fare";

        public string CostBreakdownFareHeader
        {
            get { return costBreakdownFareHeader; }
            set { costBreakdownFareHeader = value; }
        }

        private MOBItem totalPrice;

        public MOBItem TotalPrice
        {
            get { return totalPrice; }
            set { totalPrice = value; }
        }
        private string navigateToScreen;

        public string NavigateToScreen
        {
            get { return navigateToScreen; }
            set { navigateToScreen = value; }
        }
        private bool isCallRegisterOffers;
        public bool IsCallRegisterOffers
        {
            get { return isCallRegisterOffers; }
            set { isCallRegisterOffers = value; }
        }
        private List<MOBOmniCartRepricingInfo> omniCartPricingInfos;

        public List<MOBOmniCartRepricingInfo> OmniCartPricingInfos
        {
            get { return omniCartPricingInfos; }
            set { omniCartPricingInfos = value; }
        }
        private int navigateToSeatmapSegmentNumber;

        public int NavigateToSeatmapSegmentNumber
        {
            get { return navigateToSeatmapSegmentNumber; }
            set { navigateToSeatmapSegmentNumber = value; }
        }
    }

    [Serializable()]
    public class MOBPaxDetails
    {
        private string fullName;

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }
        private string seat;

        public string Seat
        {
            get { return seat; }
            set { seat = value; }
        }
        private string key;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }
    }
}

