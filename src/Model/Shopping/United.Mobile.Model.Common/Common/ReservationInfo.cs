using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.SSR;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ReservationInfo
    {
        public bool IsCorporateBooking { get; set; }

        public string CorporateRate { get; set; } = string.Empty;

        public string CorporateBookingConfirmationMessage { get; set; } = string.Empty;

        public string CorporateSuppressSavedTravelerMessage { get; set; } = string.Empty;

        public bool CanHideSelectFOPOptionsAndAddCreditCard { get; set; }

        public MOBTripInsuranceInfo TripInsuranceInfo { get; set; }

        public bool IsForceSeatMap { get; set; }

        private bool isGhostCardValidForTPIPurchase = true;
        public bool IsGhostCardValidForTPIPurchase
        {
            get { return isGhostCardValidForTPIPurchase; }
            set { isGhostCardValidForTPIPurchase = value; }
        }

    }

    [Serializable()]
    public class ReservationInfo2
    {
        public bool IsDisplayCart { get; set; }

        public bool IsCovidTestFlight { get; set; }

        public string BookingCutOffMinutes { get; set; }
        [XmlArrayItem("MOBSection")]
        public List<Section> AlertMessages { get; set; }

        public List<MOBMobileCMSContentMessages> ConfirmationPageAlertMessages { get; set; }

        public bool ShowSelectDifferentFOPAtRTI { get; set; }

        public bool IsArrangerBooking { get; set; }

        public string BundleCartID { get; set; }

        public bool IsYATravel { get; set; }

        [XmlArrayItem("MOBDisplayTravelType")]
        [XmlArray("DisplayTravelTypes")]
        public List<DisplayTravelType> displayTravelTypes { get; set; }

        public int TravelOptionEligibleTravelersCount { get; set; }

        public List<MOBTravelerType> TravelerTypes { get; set; }

        public MOBCCAdStatement ChaseCreditStatement { get; set; }
        public PlacePass PlacePass { get; set; }

        public InfoNationalityAndResidence InfoNationalityAndResidence { get; set; }
        public bool IsForceSeatMapInRTI { get; set; }

        public bool IsForceSeatMap { get; set; }

        private bool isPaxRegistered;
        public bool IsPaxRegistered
        {
            get { return isPaxRegistered; }
            set { isPaxRegistered = value; }
        }

        #region 159514 - Added for Inhibit booking message  

        
        private bool enableFinalBookingAgreenPurchaseButton = true;
        //FALSE when depature time is less than 30 minutes
        public bool EnableFinalBookingAgreenPurchaseButton
        {
            get { return enableFinalBookingAgreenPurchaseButton; }
            set { enableFinalBookingAgreenPurchaseButton = value; }
        }

        #region 177113 - 179536 BE Fare Inversion and stacking messages 
        [XmlArrayItem("MOBInfoWarningMessages")]
        public List<InfoWarningMessages> InfoWarningMessages { get; set; }

        public List<InfoWarningMessages> InfoWarningMessagesPayment { get; set; }

        #endregion
        public List<MOBCPTraveler> AllEligibleTravelersCSL { get; set; }

        public List<RTIMandateContentToDisplayByMarket> RTIMandateContentsToDisplayByMarket { get; set; }


        public string NextViewName { get; set; } = string.Empty;

        public ReservationInfo2()
        {
            TravelerTypes = new List<MOBTravelerType>();
        }


        #endregion

        #region 214448 - Unaccompained Minor Age (UMNR)

        public ReservationAgeBoundInfo ReservationAgeBoundInfo { get; set; }

        #endregion

        public bool IsShowBookingBundles { get; set; }=true;

        public bool ShouldHideBackButton { get; set; }

        public PriorityBoarding PriorityBoarding { get; set; }

        public bool IsUnfinihedBookingPath { get; set; }

        public bool IsIBELite { get; set; }

        public bool IsIBE { get; set; }

        public string FareRestrictionsMessage { get; set; } 

        public TravelSpecialNeeds SpecialNeeds { get; set; }

        public bool PurchaseToTravelTimeIsWithinSevenDays { get; set; }

        public string SelectTravelersHeaderText { get; set; }
        private List<MOBItem> characteristics;
        //public List<MOBItem> Characteristics { get; set; }
        public List<MOBItem> Characteristics
        {
            get { return characteristics; }
            set { characteristics = value; }
        }

        public bool HideSelectSeatsOnRTI { get; set; }

        public bool HideTravelOptionsOnRTI { get; set; }

        public MOBCreditCard Uplift { get; set; }

        private bool isMultipleTravelerEtcFeatureClientToggleEnabled = false;
        public bool IsMultipleTravelerEtcFeatureClientToggleEnabled
        {
            get
            {
                return this.isMultipleTravelerEtcFeatureClientToggleEnabled;
            }
            set
            {
                this.isMultipleTravelerEtcFeatureClientToggleEnabled = value;
            }
        }

        public string TravelType { get; set; }

        //This creating issue for MSC Payment and breaking the reservation.shoppingreservationinfo2 session
        //public MOBFSRAlertMessage ConfirmationScreenAlertMessages { get; set; }
        public MOBAlertMessages ConfirmationScreenAlertMessages { get; set; }

        private bool isOmniCartSavedTrip; //This boolean will be set only when omnicart saved trip has travelers

        public bool IsOmniCartSavedTrip
        {
            get { return isOmniCartSavedTrip; }
            set { isOmniCartSavedTrip = value; }
        }
        private bool isOmniCartSavedTripFlow; //This boolean will be set only when omnicart saved trip has travelers

        public bool IsOmniCartSavedTripFlow
        {
            get { return isOmniCartSavedTripFlow; }
            set { isOmniCartSavedTripFlow = value; }
        }

        private bool isSelectSeatsFromRTI;
        public bool IsSelectSeatsFromRTI
        {
            get { return isSelectSeatsFromRTI; }
            set { isSelectSeatsFromRTI = value; }
        }
        private int seatRemoveCouponPopupCount;

        public int SeatRemoveCouponPopupCount
        {
            get { return seatRemoveCouponPopupCount; }
            set { seatRemoveCouponPopupCount = value; }
        }
        public MOBVisaCheckout VisaCheckOutDetails { get; set; }
        private bool hideBackButtonOnSelectTraveler;

        public bool HideBackButtonOnSelectTraveler
        {
            get { return hideBackButtonOnSelectTraveler; }
            set { hideBackButtonOnSelectTraveler = value; }
        }
        private bool isNonRefundableNonChangable;
        public bool IsNonRefundableNonChangable
        {
            get { return isNonRefundableNonChangable; }
            set { isNonRefundableNonChangable = value; }
        }
        private string corporateDisclaimerText;
        public string CorporateDisclaimerText
        {
            get { return corporateDisclaimerText; }
            set { corporateDisclaimerText = value; }
        }
    }

    #region 177113 - 179536 BE Fare Inversion and stacking messages 
    public enum INFOWARNINGMESSAGEORDER
    {
        RTITRAVELADVISORY,
        BOEING737WARNING,
        INHIBITBOOKING,
        UPLIFTTPISECONDARYPAYMENT,
        CONCURRCARDPOLICY,
        BEFAREINVERSION,
        PRICECHANGE,
        RTITRAVELADVISORYBROKENANDRIODVERSION,
        RTIETCBALANCEATTENTION,
        RTIPROMOSELECTSEAT,
        RTICORPORATELEISUREOPTOUTMESSAGE,
        UPSELLSEATMAPMESSAGE
    }
    public enum CONFIRMATIONALERTMESSAGEORDER
    {
        SPECIALNEEDS,
        COVIDTESTINFO,
        ADVISORY,
        FACECOVERING,
        TRIPADVISORY,
        SEATASSIGNMENTFAILURE,
        TRIPINSURANCEFAILURE,
        TRAINEDDOG,
        TRAVELCERTIFICATEBALANCE,
        SAVEWALLETFAILURE,
        RESERVATIONON24HOURHOLD
    }
    public enum MESSAGETYPES
    {
        INFORMATION,
        WARNING,
        CAUTION
    }

    public enum DeeplinkType
    {
        TRIPSHARE
    }

    public enum FSRINFOMSGORDER
    {
        YOUNGADULT
    }

    public enum INFOWARNINGMESSAGEICON
    {
        INFORMATION,
        WARNING,
        SUCCESS
    }

    public enum ShoppingExperiments
    {
        NoChangeFee,
        FSRRedesignA,
        FSRRedesignAward
    }

    public enum CORPORATEBOOKINGTYPE
    {
        CorporateBusiness,
        TravelArranger,
        CorpLeisure
    }

    [Serializable()]
    public class InfoWarningMessages
    {

        public string Order { get; set; } = string.Empty;

        public string IconType { get; set; } = string.Empty;

        public List<string> Messages { get; set; }

        public string ButtonLabel { get; set; }

        public string HeaderMessage { get; set; } 

        public bool IsCollapsable { get; set; }

        public bool IsExpandByDefault { get; set; }

        public InfoWarningMessages()
        {
            Messages = new List<string>();
        }
    }
    #endregion

    [Serializable()]
    public class InfoNationalityAndResidence
    {
        public string NationalityErrMsg { get; set; } 

        public string ResidenceErrMsg { get; set; } 

        public string NationalityAndResidenceErrMsg { get; set; } 

        public string NationalityAndResidenceHeaderMsg { get; set; } 

        public List<List<MOBSHOPTax>> ComplianceTaxes { get; set; }

        public bool IsRequireNationalityAndResidence { get; set; }

        public InfoNationalityAndResidence()
        {
            ComplianceTaxes = new List<List<MOBSHOPTax>>();
        }

    }

    public static class NationalityResidenceMsgs
    {
        private static string nationalityErrMsg;
        private static string residenceErrMsg;
        private static string nationalityAndResidenceErrMsg;
        private static string nationalityAndResidenceHeaderMsg;
        public static string NationalityAndResidenceHeaderMsg { get { return nationalityAndResidenceHeaderMsg; } }
        public static string NationalityErrMsg { get { return nationalityErrMsg; } }
        public static string ResidenceErrMsg { get { return residenceErrMsg; } }
        public static string NationalityAndResidenceErrMsg { get { return nationalityAndResidenceErrMsg; } }


        //static NationalityResidenceMsgs()
        //{
        //    //TODO: Elgendy Configsssss
        //    //nationalityErrMsg = ConfigurationManager.AppSettings["NationalityErrMsg"] ?? string.Empty;
        //    //residenceErrMsg = ConfigurationManager.AppSettings["ResidenceErrMsg"] ?? string.Empty;
        //    //nationalityAndResidenceErrMsg = ConfigurationManager.AppSettings["NationalityAndResidenceErrMsg"] ?? string.Empty;
        //    //nationalityAndResidenceHeaderMsg = ConfigurationManager.AppSettings["NationalityAndResidenceHeaderMsg"] ?? string.Empty;
        //}
        public static void LoadConfig(IConfiguration configuration)
        {
            nationalityErrMsg = configuration.GetValue<string>("NationalityErrMsg") ?? string.Empty;
            residenceErrMsg = configuration.GetValue<string>("ResidenceErrMsg") ?? string.Empty;
            nationalityAndResidenceErrMsg = configuration.GetValue<string>("NationalityAndResidenceErrMsg") ?? string.Empty;
            nationalityAndResidenceHeaderMsg = configuration.GetValue<string>("NationalityAndResidenceHeaderMsg") ?? string.Empty;
        }
    }

    #region 214448 - Unaccompained Minor Age (UMNR)
    [Serializable()]
    public class ReservationAgeBoundInfo
    {

        public int MinimumAge { get; set; }

        public int UpBoundAge { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

    }
    #endregion


    #region MB 804 - Chase promo RTI

    public class MOBCCAdStatement
    {
        [JsonIgnore()]
        public string ObjectName { get; set; } = "United.Definition.Shopping.MOBCCAdStatement";
        public string initialDisplayPrice { get; set; }
        public string statementCreditDisplayPrice { get; set; }
        public string finalAfterStatementDisplayPrice { get; set; }
        public Image bannerImage { get; set; }
        public string ccImage { get; set; }
        public CCAdCCEPromoBanner chaseBanner { get; set; }
        public string styledInitialDisplayPrice { get; set; }
        public string styledInitialDisplayText { get; set; }
        public string styledStatementCreditDisplayPrice { get; set; }
        public string styledStatementCreditDisplayText { get; set; }
        public string styledFinalAfterStatementDisplayPrice { get; set; }
        public string styledFinalAfterStatementDisplayText { get; set; }

    }

    [Serializable()]
    public class CCAdCCEPromoBanner
    {
        private string messageKey;
        private Image bannerImage;
        private bool makeFeedBackCall;
        private bool displayPriceCalculation;

        public string MessageKey { get => messageKey; set => messageKey = value; }
        public Image BannerImage { get => bannerImage; set => bannerImage = value; }
        public bool MakeFeedBackCall { get => makeFeedBackCall; set => makeFeedBackCall = value; }
        public bool DisplayPriceCalculation { get => displayPriceCalculation; set => displayPriceCalculation = value; }

        private string placementLandingPageURL;

        public string PlacementLandingPageURL
        {
            get { return placementLandingPageURL; }
            set { placementLandingPageURL = value; }
        }

    }

    [Serializable()]
    public class Image
    {
        string phoneUrl;
        string tabletUrl;

        public string PhoneUrl
        {
            get { return phoneUrl; }
            set { phoneUrl = value; }

        }

        public string TabletUrl
        {
            get { return tabletUrl; }
            set { tabletUrl = value; }

        }
    }

    public enum CHASEADTYPE
    {
        NONE,
        NONPREMIER,
        PREMIER
    }
    public enum MOBINFOWARNINGMESSAGEICON
    {
        INFORMATION,
        WARNING,
        SUCCESS
    }
    public enum MOBINFOWARNINGMESSAGEORDER
    {
        RTITRAVELADVISORY,
        BOEING737WARNING,
        INHIBITBOOKING,
        UPLIFTTPISECONDARYPAYMENT,
        CONCURRCARDPOLICY,
        BEFAREINVERSION,
        PRICECHANGE,
        RTITRAVELADVISORYBROKENANDRIODVERSION,
        RTIETCBALANCEATTENTION,
        RTIPROMOSELECTSEAT,
        RTICORPORATELEISUREOPTOUTMESSAGE
    }
    public enum MOBCONFIRMATIONALERTMESSAGEORDER
    {
        SPECIALNEEDS,
        COVIDTESTINFO,
        ADVISORY,
        FACECOVERING,
        TRIPADVISORY,
        SEATASSIGNMENTFAILURE,
        TRIPINSURANCEFAILURE,
        TRAINEDDOG,
        TRAVELCERTIFICATEBALANCE,
        SAVEWALLETFAILURE,
        RESERVATIONON24HOURHOLD
    }
    public enum MOBMESSAGETYPES
    {
        INFORMATION,
        WARNING,
        CAUTION
    }
    #endregion
}
