using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.TravelCredit
{
    [Serializable()]
    public class MOBSHOPReservationInfo
    {
        private bool isCorporateBooking;
        public bool IsCorporateBooking
        {
            get { return isCorporateBooking; }
            set { isCorporateBooking = value; }
        }
        private string corporateRate = string.Empty;

        public string CorporateRate
        {
            get { return corporateRate; }
            set
            {
                corporateRate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string corporateBookingConfirmationMessage;
        public string CorporateBookingConfirmationMessage
        {
            get { return corporateBookingConfirmationMessage; }
            set { corporateBookingConfirmationMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string corporateSuppressSavedTravelerMessage = string.Empty;
        public string CorporateSuppressSavedTravelerMessage
        {
            get { return corporateSuppressSavedTravelerMessage; }
            set { corporateSuppressSavedTravelerMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private bool canHideSelectFOPOptionsAndAddCreditCard;

        public bool CanHideSelectFOPOptionsAndAddCreditCard
        {
            get { return canHideSelectFOPOptionsAndAddCreditCard; }
            set { canHideSelectFOPOptionsAndAddCreditCard = value; }
        }
        private MOBTripInsuranceInfo tripInsuranceInfo;
        public MOBTripInsuranceInfo TripInsuranceInfo
        {
            get { return this.tripInsuranceInfo; }
            set { this.tripInsuranceInfo = value; }
        }
        private bool isForceSeatMap = false;
        public bool IsForceSeatMap
        {
            get { return isForceSeatMap; }
            set { isForceSeatMap = value; }
        }

        private bool isGhostCardValidForTPIPurchase = true;
        public bool IsGhostCardValidForTPIPurchase
        {
            get { return isGhostCardValidForTPIPurchase; }
            set { isGhostCardValidForTPIPurchase = value; }
        }


    }

    [Serializable()]
    public class MOBSHOPReservationInfo2
    {
        private bool isForceSeatMap = false;
        private bool isPaxRegistered;
        private string nextViewName;
        private bool isForceSeatMapInRTI ;
        private bool isShowBookingBundles ;
        private bool shouldHideBackButton;
        private MOBPriorityBoarding priorityBoarding;
        private bool isUnfinihedBookingPath;
        private bool isIBELite;
        private bool isIBE;
        private string fareRestrictionsMessage;
        private MOBTravelSpecialNeeds specialNeeds;
        private bool purchaseToTravelTimeIsWithinSevenDays;

        private MOBInfoNationalityAndResidence infoNationalityAndResidence;
        private MOBVisaCheckout visaCheckOutDetails;
        private MOBPlacePass placePass;

        private MOBCCAdStatement chaseCreditStatement;
        private List<MOBTravelerType> travelerTypes;
        private List<MOBDisplayTravelType> displayTravelTypes;
        private List<MOBItem> characteristics;

        private string selectTravelersHeaderText;
        private int travelOptionEligibleTravelersCount;
        private bool hideTravelOptionsOnRTI;
        private bool hideSelectSeatsOnRTI;
        private MOBCreditCard uplift;
        private bool isMultipleTravelerEtcFeatureClientToggleEnabled;
        private bool isArrangerBooking;
        private bool showSelectDifferentFOPAtRTI;
        private List<MOBMobileCMSContentMessages> confirmationPageAlertMessages;
        private List<MOBSection> alertMessages;
        private string bookingCutOffMinutes;
        private bool isYATravel;
        private bool isDisplayCart;

        public bool IsDisplayCart
        {
            get { return isDisplayCart; }
            set { isDisplayCart = value; }
        }

        public bool IsYATravel
        {
            get { return isYATravel; }
            set { isYATravel = value; }
        }
        private bool isCovidTestFlight;

        public bool IsCovidTestFlight
        {
            get { return isCovidTestFlight; }
            set { isCovidTestFlight = value; }
        }

        public string BookingCutOffMinutes
        {
            get { return bookingCutOffMinutes; }
            set { bookingCutOffMinutes = value; }
        }

        public List<MOBSection> AlertMessages
        {
            get { return alertMessages; }
            set { alertMessages = value; }
        }

        public List<MOBMobileCMSContentMessages> ConfirmationPageAlertMessages
        {
            get
            {
                return this.confirmationPageAlertMessages;
            }
            set
            {
                this.confirmationPageAlertMessages = value;
            }
        }
        public bool ShowSelectDifferentFOPAtRTI
        {
            set
            {
                showSelectDifferentFOPAtRTI = value;
            }
            get
            {
                return showSelectDifferentFOPAtRTI;
            }
        }

        public bool IsArrangerBooking
        {
            set { isArrangerBooking = value; }
            get { return isArrangerBooking; }
        }

        public List<MOBDisplayTravelType> DisplayTravelTypes
        {
            get { return displayTravelTypes; }
            set { displayTravelTypes = value; }
        }

        public int TravelOptionEligibleTravelersCount
        {
            get { return travelOptionEligibleTravelersCount; }
            set { travelOptionEligibleTravelersCount = value; }
        }

        public List<MOBTravelerType> TravelerTypes
        {
            get { return travelerTypes; }
            set { travelerTypes = value; }
        }

        public MOBCCAdStatement ChaseCreditStatement
        {
            get
            {
                return this.chaseCreditStatement;
            }
            set
            {
                this.chaseCreditStatement = value;
            }
        }

        public MOBPlacePass PlacePass
        {
            get
            {
                return placePass;
            }
            set
            {
                this.placePass = value;
            }
        }
        public MOBVisaCheckout VisaCheckOutDetails
        {
            get { return visaCheckOutDetails; }
            set { visaCheckOutDetails = value; }
        }

        public MOBInfoNationalityAndResidence InfoNationalityAndResidence
        {
            get { return infoNationalityAndResidence; }
            set { infoNationalityAndResidence = value; }
        }


        public bool IsForceSeatMapInRTI
        {
            get { return isForceSeatMapInRTI; }
            set { isForceSeatMapInRTI = value; }
        }


        public bool IsForceSeatMap
        {
            get { return isForceSeatMap; }
            set { isForceSeatMap = value; }
        }

        public bool IsPaxRegisterd
        {
            get { return isPaxRegistered; }
            set { isPaxRegistered = value; }
        }
        #region 159514 - Added for Inhibit booking message  

        private bool enableFinalBookingAgreenPurchaseButton;
        //FALSE when depature time is less than 30 minutes
        public bool EnableFinalBookingAgreenPurchaseButton
        {
            get { return enableFinalBookingAgreenPurchaseButton; }
            set { enableFinalBookingAgreenPurchaseButton = value; }
        }

        #region 177113 - 179536 BE Fare Inversion and stacking messages 
        private List<MOBInfoWarningMessages> infoWarningMessages;
        public List<MOBInfoWarningMessages> InfoWarningMessages
        {
            get { return infoWarningMessages; }
            set { infoWarningMessages = value; }
        }
        #endregion

        private List<MOBRTIMandateContentToDisplayByMarket> rtiMandateContentsToDisplayByMarket;
        public List<MOBRTIMandateContentToDisplayByMarket> RTIMandateContentsToDisplayByMarket
        {
            get { return rtiMandateContentsToDisplayByMarket; }
            set { rtiMandateContentsToDisplayByMarket = value; }
        }

        private List<MOBCPTraveler> allEligibleTravelersCSL;
        public List<MOBCPTraveler> AllEligibleTravelersCSL
        {
            get
            {
                return this.allEligibleTravelersCSL;
            }
            set
            {
                this.allEligibleTravelersCSL = value;
            }
        }

        public string NextViewName
        {
            get { return nextViewName; }
            set { nextViewName = value; }
        }



        #endregion

        #region 214448 - Unaccompained Minor Age (UMNR)

        private MOBReservationAgeBoundInfo reservationAgeBoundInfo;
        public MOBReservationAgeBoundInfo ReservationAgeBoundInfo
        {
            get { return reservationAgeBoundInfo; }
            set { reservationAgeBoundInfo = value; }
        }
        #endregion

        public bool IsShowBookingBundles
        {
            get { return isShowBookingBundles; }
            set { isShowBookingBundles = value; }
        }

        public bool ShouldHideBackButton
        {
            get { return shouldHideBackButton; }
            set { shouldHideBackButton = value; }
        }

        public MOBPriorityBoarding PriorityBoarding
        {
            get { return priorityBoarding; }
            set { priorityBoarding = value; }
        }

        public bool IsUnfinihedBookingPath
        {
            get { return isUnfinihedBookingPath; }
            set { isUnfinihedBookingPath = value; }
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

        public string FareRestrictionsMessage
        {
            get { return fareRestrictionsMessage; }
            set { fareRestrictionsMessage = value; }
        }

        public MOBTravelSpecialNeeds SpecialNeeds
        {
            get { return specialNeeds; }
            set { specialNeeds = value; }
        }

        public bool PurchaseToTravelTimeIsWithinSevenDays
        {
            get { return purchaseToTravelTimeIsWithinSevenDays; }
            set { purchaseToTravelTimeIsWithinSevenDays = value; }
        }

        public string SelectTravelersHeaderText
        {
            get { return selectTravelersHeaderText; }
            set { selectTravelersHeaderText = value; }
        }


        public List<MOBItem> Characteristics
        {
            get { return characteristics; }
            set { characteristics = value; }
        }

        public bool HideSelectSeatOnRTI
        {
            get { return hideSelectSeatsOnRTI; }
            set { hideSelectSeatsOnRTI = value; }
        }


        public bool HideTravelOptionsOnRTI
        {
            get { return hideTravelOptionsOnRTI; }
            set { hideTravelOptionsOnRTI = value; }
        }

        public MOBCreditCard Uplift
        {
            get { return uplift; }
            set { uplift = value; }
        }
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
        private string travelType;

        public string TravelType
        {
            get { return travelType; }
            set { travelType = value; }
        }
        private MOBAlertMessages confirmationScreenAlertMessages;

        public MOBAlertMessages ConfirmationScreenAlertMessages
        {
            get { return confirmationScreenAlertMessages; }
            set { confirmationScreenAlertMessages = value; }
        }
        private int seatRemoveCouponPopupCount;

        public int SeatRemoveCouponPopupCount
        {
            get { return seatRemoveCouponPopupCount; }
            set { seatRemoveCouponPopupCount = value; }
        }

        private bool isSelectSeatsFromRTI;

        public bool IsSelectSeatsFromRTI
        {
            get { return isSelectSeatsFromRTI; }
            set { isSelectSeatsFromRTI = value; }
        }
        private bool isOmniCartSavedTrip;

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
    }

    #region 177113 - 179536 BE Fare Inversion and stacking messages 
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
        RTIPROMOSELECTSEAT

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
    public enum MOBINFOWARNINGMESSAGEICON
    {
        INFORMATION,
        WARNING
    }

    [Serializable()]
    public class MOBInfoWarningMessages
    {

        private string order;
        public string Order
        {
            get { return order; }
            set { order = value; }
        }

        private string iconType;
        public string IconType
        {
            get { return iconType; }
            set { iconType = value; }
        }

        private List<string> messages;

        public List<string> Messages
        {
            get { return messages; }
            set { messages = value; }
        }
        private string buttonLabel;

        public string ButtonLabel
        {
            get { return buttonLabel; }
            set { buttonLabel = value; }
        }
        private string headerMessage;
        public string HeaderMessage
        {
            get { return headerMessage; }
            set { headerMessage = value; }
        }
        private bool isCollapsable;

        public bool IsCollapsable
        {
            get { return isCollapsable; }
            set { isCollapsable = value; }
        }
        private bool isExpandByDefault;

        public bool IsExpandByDefault
        {
            get { return isExpandByDefault; }
            set { isExpandByDefault = value; }
        }
    }
    #endregion

    [Serializable()]
    public class MOBInfoNationalityAndResidence
    {

        //For Nationality And Country Of Residence
        private bool isRequireNationalityAndResidence;
        private string nationalityErrMsg;
        private string residenceErrMsg;
        private string nationalityAndResidenceErrMsg;
        private string nationalityAndResidenceHeaderMsg;

        public string NationalityErrMsg
        {
            get
            {
                return nationalityErrMsg;
            }
            set
            {
                nationalityErrMsg = value;
            }
        }

        public string ResidenceErrMsg
        {
            get
            {
                return residenceErrMsg;
            }
            set
            {
                residenceErrMsg = value;
            }
        }
        public string NationalityAndResidenceErrMsg
        {
            get
            {
                return nationalityAndResidenceErrMsg;
            }
            set
            {
                nationalityAndResidenceErrMsg = value;
            }
        }

        public string NationalityAndResidenceHeaderMsg
        {
            get { return nationalityAndResidenceHeaderMsg; }
            set { nationalityAndResidenceHeaderMsg = value; }
        }
        private List<List<MOBSHOPTax>> complianceTaxes = null;

        public List<List<MOBSHOPTax>> ComplianceTaxes
        {
            get { return complianceTaxes; }
            set { complianceTaxes = value; }
        }
        public bool IsRequireNationalityAndResidence
        {
            get { return isRequireNationalityAndResidence; }
            set { isRequireNationalityAndResidence = value; }
        }

    }

    public class MOBNationalityResidenceMsgs
    {
        private static readonly string nationalityErrMsg;
        private static readonly string residenceErrMsg;
        private static readonly string nationalityAndResidenceErrMsg;
        private static readonly string nationalityAndResidenceHeaderMsg;
        public static string NationalityAndResidenceHeaderMsg { get { return nationalityAndResidenceHeaderMsg; } }
        public static string NationalityErrMsg { get { return nationalityErrMsg; } }
        public static string ResidenceErrMsg { get { return residenceErrMsg; } }
        public static string NationalityAndResidenceErrMsg { get { return nationalityAndResidenceErrMsg; } }


        static MOBNationalityResidenceMsgs()
        {
            nationalityErrMsg = ConfigurationManager.AppSettings["NationalityErrMsg"] ?? string.Empty;
            residenceErrMsg = ConfigurationManager.AppSettings["ResidenceErrMsg"] ?? string.Empty;
            nationalityAndResidenceErrMsg = ConfigurationManager.AppSettings["NationalityAndResidenceErrMsg"] ?? string.Empty;
            nationalityAndResidenceHeaderMsg = ConfigurationManager.AppSettings["NationalityAndResidenceHeaderMsg"] ?? string.Empty;
        }
    }

    #region 214448 - Unaccompained Minor Age (UMNR)
    [Serializable()]
    public class MOBReservationAgeBoundInfo
    {

        private int minimumAge;
        public int MinimumAge
        {
            get { return minimumAge; }
            set { minimumAge = value; }
        }

        private int upBoundAge;
        public int UpBoundAge
        {
            get { return upBoundAge; }
            set { upBoundAge = value; }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }
    }
    #endregion


    #region MB 804 - Chase promo RTI

    public class MOBCCAdStatement
    {
        public string initialDisplayPrice { get; set; }
        public string statementCreditDisplayPrice { get; set; }
        public string finalAfterStatementDisplayPrice { get; set; }
        public MOBImage bannerImage { get; set; }
        public string ccImage { get; set; }
        public MOBCCAdCCEPromoBanner chaseBanner { get; set; }
        public string styledInitialDisplayPrice { get; set; }
        public string styledInitialDisplayText { get; set; }
        public string styledStatementCreditDisplayPrice { get; set; }
        public string styledStatementCreditDisplayText { get; set; }
        public string styledFinalAfterStatementDisplayPrice { get; set; }
        public string styledFinalAfterStatementDisplayText { get; set; }
    }

    [Serializable()]
    public class MOBCCAdCCEPromoBanner
    {
        private string messageKey;
        private MOBImage bannerImage;
        private bool makeFeedBackCall;
        private bool displayPriceCalculation;

        public string MessageKey { get => messageKey; set => messageKey = value; }
        public MOBImage BannerImage { get => bannerImage; set => bannerImage = value; }
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
    public class MOBImage
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
    #endregion
}
