using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Definition.CancelReservation;
//using United.Definition.Shopping;

namespace United.Mobile.Model.PNRManagement
{
    [Serializable()]
    public class MOBPNR
    {
        private string sessionId = string.Empty;
        private string recordLocator = string.Empty;
        private string dateCreated = string.Empty;
        private string description = string.Empty;
        private bool isActive;
        private string ticketType = string.Empty;
        private string numberOfPassengers = string.Empty;
        private List<MOBTrip> trips;
        private List<MOBPNRPassenger> passengers;
        private List<MOBPNRSegment> segments;
        private string lastTripDateDepartureDate = string.Empty;
        private string lastTripDateArrivalDate = string.Empty;
        private string checkinEligible = "N";
        private List<MOBPNRPassenger> infantInLaps;
        private string alreadyCheckedin = "false";
        private string notValid = "false";
        private string validforCheckin = "false";
        private string pnrCanceled = "false";
        private string uaRecordLocator = string.Empty;
        private string coRecordLocator = string.Empty;
        private string pnrOwner = string.Empty;
        private string mealAccommodationAdvisory = string.Empty;
        private string mealAccommodationAdvisoryHeader = string.Empty;
        private List<MOBOARecordLocator> oaRecordLocators = new List<MOBOARecordLocator>();
        private string oaRecordLocatorMessageTitle = string.Empty;
        private string oaRecordLocatorMessage = string.Empty;
        private bool isEligibleToSeatChange = false;
        private string emailAddress = string.Empty;
        private MOBSeatOffer seatOffer;
        private Boolean isShuttleOfferEligible;
        private Boolean isETicketed;
        private Boolean shouldDisplayEmailReceipt;
        private Boolean isPetAvailable;
        private MOBBundleInfo bundleInfo;
        private Boolean showOverride24HrFlex;
        private MOBScheduleChange scheduleChangeInfo;
        private MOBIRROPSChange irropsChangeInfo;

        ////private bool isInCabinPetInReservation; //Kept Commented If need in future about More pet information

        ////private List<PetInformation> pets; //Kept Commented If need in future about More pet information
        private string associateMPId = "false";
        private string associateMPIdSharesPosition = string.Empty;
        private string associateMPIdSharesGivenName = string.Empty;
        private string associateMPIdMessage = string.Empty;

        private List<string> petRecordLocators;
        private string upgradeMessage = string.Empty;
        private string farelockExpirationDate = string.Empty;
        private string farelockPurchaseMessage = string.Empty;
        private string earnedMilesHeader = string.Empty;
        private string earnedMilesText = string.Empty;
        private string ineligibleToEarnCreditMessage = string.Empty;
        private string oaIneligibleToEarnCreditMessage = string.Empty;
        private bool awardTravel;
        private bool psSaTravel;
        private bool supressLMX = false;
        private bool isELF = false;
        private List<MOBItem> elfLimitations;

        private string overMileageLimitMessage = ConfigurationManager.AppSettings["lmxOverMileageLimitMessage"];
        private string overMileageLimitAmount = ConfigurationManager.AppSettings["lmxOverMileageLimitAmount"];

        private List<MOBLMXTraveler> lmxtravelers;

        private string tripType = string.Empty;
        private bool isTPIIncluded = false;
        private bool isFareLockOrNRSA = false;

        private bool hasCheckedBags;
        private bool isIBELite;
        private bool isIBE;

        private bool hasScheduleChanged;
        private List<MOBItem> statusMessageItems;

        private string productCategory;
        private string productCode;
        private string marketType;
        private string journeyType;
        private string checkInStatus = string.Empty;
        private bool getCheckInStatusFromCSLPNRRetrivalService; // This Flag to check the CheckInEligibility Check already done at CSL PNR Retrival Service (as first phase the CSL PNR Team implemented at CSL PNR Retrival by RecordLocattor second phase they do at GetPNRByMP)
        private bool irrOps;
        private bool irrOpsViewed;
        private bool isEnableEditTraveler;
        private bool isUnaccompaniedMinor;
        private bool isCanceledWithFutureFlightCredit;
        private MOBUmnr umnr;
        private MOBInCabinPet inCabinPetInfo;
        private MOBFutureFlightCredit futureflightcredit;
        private List<MOBShuttleOffer> shuttleOfferInformation;
        private List<MOBItem> earnedMilesInfo;
        private bool isCheckinEligible;
        private bool isAgencyBooking;
        private string agencyName;
        private bool isPolicyExceptionAlert;
        private bool isBEChangeEligible;
        private bool shouldDisplayUpgradeCabin;
        private bool isCorporateBooking;
        private string corporateVendorName;
        private List<MOBPNRAdvisory> advisoryInfo;
        private bool consolidateScheduleChangeMessage;
        private List<MOBReservationPrice> prices;
        private bool isTicketedByUA;
        private bool isSCChangeEligible;
        private bool isSCRefundEligible;
        private bool isSCBulkGroupPWC;

        private MOBOneClickEnrollmentEligibility oneClickEnrollmentEligibility;


        private bool isATREEligible;

        public Boolean IsATREEligible { get { return this.isATREEligible; } set { this.isATREEligible = value; } }
        public Boolean IsTicketedByUA { get { return this.isTicketedByUA; } set { this.isTicketedByUA = value; } }
        public Boolean IsSCChangeEligible { get { return this.isSCChangeEligible; } set { this.isSCChangeEligible = value; } }
        public Boolean IsSCRefundEligible { get { return this.isSCRefundEligible; } set { this.isSCRefundEligible = value; } }
        public Boolean IsSCBulkGroupPWC { get { return this.isSCBulkGroupPWC; } set { this.isSCBulkGroupPWC = value; } }
        public MOBScheduleChange ScheduleChangeInfo { get { return this.scheduleChangeInfo; } set { this.scheduleChangeInfo = value; } }
        public MOBIRROPSChange IRROPSChangeInfo { get { return this.irropsChangeInfo; } set { this.irropsChangeInfo = value; } }
        public List<MOBPNRAdvisory> AdvisoryInfo { get { return this.advisoryInfo; } set { this.advisoryInfo = value; } }
        public Boolean ConsolidateScheduleChangeMessage { get { return this.consolidateScheduleChangeMessage; } set { this.consolidateScheduleChangeMessage = value; } }
        public Boolean IsCorporateBooking { get { return this.isCorporateBooking; } set { this.isCorporateBooking = value; } }
        public string CorporateVendorName { get { return this.corporateVendorName; } set { this.corporateVendorName = value; } }
        public Boolean IsPolicyExceptionAlert { get { return this.isPolicyExceptionAlert; } set { this.isPolicyExceptionAlert = value; } }
        public Boolean IsBEChangeEligible { get { return this.isBEChangeEligible; } set { this.isBEChangeEligible = value; } }
        public Boolean IsCheckinEligible { get { return this.isCheckinEligible; } set { this.isCheckinEligible = value; } }
        public Boolean IsAgencyBooking { get { return this.isAgencyBooking; } set { this.isAgencyBooking = value; } }
        public string AgencyName { get { return this.agencyName; } set { this.agencyName = value; } }
        public Boolean ShouldDisplayUpgradeCabin { get { return this.shouldDisplayUpgradeCabin; } set { this.shouldDisplayUpgradeCabin = value; } }
        public List<MOBItem> EarnedMilesInfo { get { return this.earnedMilesInfo; } set { this.earnedMilesInfo = value; } }
        public List<MOBShuttleOffer> ShuttleOfferInformation { get { return this.shuttleOfferInformation; } set { this.shuttleOfferInformation = value; } }
        public MOBFutureFlightCredit FutureFlightCredit { get { return this.futureflightcredit; } set { this.futureflightcredit = value; } }
        public MOBUmnr Umnr { get { return this.umnr; } set { this.umnr = value; } }
        public List<MOBPNRPassenger> InfantInLaps { get { return this.infantInLaps; } set { this.infantInLaps = value; } }
        public string MealAccommodationAdvisory { get { return this.mealAccommodationAdvisory; } set { this.mealAccommodationAdvisory = value; } }
        public string MealAccommodationAdvisoryHeader { get { return this.mealAccommodationAdvisoryHeader; } set { this.mealAccommodationAdvisoryHeader = value; } }
        public Boolean IsUnaccompaniedMinor { get { return this.isUnaccompaniedMinor; } set { this.isUnaccompaniedMinor = value; } }
        public Boolean IsETicketed { get { return this.isETicketed; } set { this.isETicketed = value; } }
        public MOBInCabinPet InCabinPetInfo { get { return this.inCabinPetInfo; } set { this.inCabinPetInfo = value; } }
        public Boolean IsPetAvailable { get { return this.isPetAvailable; } set { this.isPetAvailable = value; } }
        public Boolean ShouldDisplayEmailReceipt { get { return this.shouldDisplayEmailReceipt; } set { this.shouldDisplayEmailReceipt = value; } }
        public Boolean IsCanceledWithFutureFlightCredit { get { return this.isCanceledWithFutureFlightCredit; } set { this.isCanceledWithFutureFlightCredit = value; } }
        public Boolean ShowOverride24HrFlex { get { return this.showOverride24HrFlex; } set { this.showOverride24HrFlex = value; } }
       
        private bool isInflightMealsOfferEligible;

        public bool IsInflightMealsOfferEligible
        {
            get { return isInflightMealsOfferEligible; }
            set { isInflightMealsOfferEligible = value; }
        }


        public bool IsEnableEditTraveler
        {
            get { return this.isEnableEditTraveler; }
            set { isEnableEditTraveler = value; }
        }

        public Boolean IsShuttleOfferEligible { get { return this.isShuttleOfferEligible; } set { this.isShuttleOfferEligible = value; } }

        public string JourneyType
        {
            get { return journeyType; }
            set { journeyType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
        public List<MOBItem> ELFLimitations
        {
            get { return elfLimitations; }
            set { elfLimitations = value; }
        }

        public bool IsElf
        {
            get { return isELF; }
            set { isELF = value; }
        }
        public List<MOBLMXTraveler> LMXTravelers
        {
            get
            {
                return this.lmxtravelers;
            }
            set
            {
                this.lmxtravelers = value;
            }
        }

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string RecordLocator
        {
            get
            {
                return this.recordLocator;
            }
            set
            {
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string UARecordLocator
        {
            get
            {
                return this.uaRecordLocator;
            }
            set
            {
                this.uaRecordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string CORecordLocator
        {
            get
            {
                return this.coRecordLocator;
            }
            set
            {
                this.coRecordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string PNROwner
        {
            get
            {
                return this.pnrOwner;
            }
            set
            {
                this.pnrOwner = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }


        public List<MOBOARecordLocator> OARecordLocators
        {
            get
            {
                return this.oaRecordLocators;
            }
            set
            {
                this.oaRecordLocators = value;
            }
        }

        public string OARecordLocatorMessageTitle
        {
            get
            {
                return this.oaRecordLocatorMessageTitle;
            }
            set
            {
                this.oaRecordLocatorMessageTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OARecordLocatorMessage
        {
            get
            {
                return this.oaRecordLocatorMessage;
            }
            set
            {
                this.oaRecordLocatorMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DateCreated
        {
            get
            {
                return this.dateCreated;
            }
            set
            {
                this.dateCreated = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                this.isActive = value;
            }
        }

        public string TicketType
        {
            get
            {
                return this.ticketType;
            }
            set
            {
                this.ticketType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string NumberOfPassengers
        {
            get
            {
                return this.numberOfPassengers;
            }
            set
            {
                this.numberOfPassengers = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public List<MOBTrip> Trips
        {
            get
            {
                return this.trips;
            }
            set
            {
                this.trips = value;
            }
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

        public string CheckinEligible
        {
            get
            {
                return this.checkinEligible;
            }
            set
            {
                this.checkinEligible = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string LastTripDateDepartureDate
        {
            get
            {
                return this.lastTripDateDepartureDate;
            }
            set
            {
                this.lastTripDateDepartureDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string LastTripDateArrivalDate
        {
            get
            {
                return this.lastTripDateArrivalDate;
            }
            set
            {
                this.lastTripDateArrivalDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string AlreadyCheckedin
        {
            get
            {
                return this.alreadyCheckedin;
            }
            set
            {
                this.alreadyCheckedin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string NotValid
        {
            get
            {
                return this.notValid;
            }
            set
            {
                this.notValid = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ValidforCheckin
        {
            get
            {
                return this.validforCheckin;
            }
            set
            {
                this.validforCheckin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PNRCanceled
        {
            get
            {
                return this.pnrCanceled;
            }
            set
            {
                this.pnrCanceled = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBSeatOffer SeatOffer
        {
            get
            {
                return this.seatOffer;
            }
            set
            {
                this.seatOffer = value;
            }
        }

        public bool IsEligibleToSeatChange
        {
            get
            {
                return this.isEligibleToSeatChange;
            }
            set
            {
                this.isEligibleToSeatChange = value;
            }
        }

        ////public bool IsInCabinPetInReservation
        ////{
        ////    get
        ////    {
        ////        return this.isInCabinPetInReservation;
        ////    }
        ////    set
        ////    {
        ////        this.isInCabinPetInReservation = value;
        ////    }
        ////}

        ////public List<PetInformation> Pets
        ////{
        ////    get
        ////    {
        ////        return this.pets;
        ////    }
        ////    set
        ////    {
        ////        pets = value;
        ////    }
        ////}

        public List<string> PetRecordLocators
        {
            get
            {
                return this.petRecordLocators;
            }
            set
            {
                petRecordLocators = value;
            }
        }

        public string EmailAddress
        {
            get
            {
                return this.emailAddress;
            }
            set
            {
                this.emailAddress = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string UpgradeMessage
        {
            get
            {
                return this.upgradeMessage;
            }
            set
            {
                this.upgradeMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBBundleInfo BundleInfo
        {
            get
            {
                return this.bundleInfo;
            }
            set
            {
                this.bundleInfo = value;
            }
        }

        public string FarelockExpirationDate
        {
            get
            {
                return this.farelockExpirationDate;
            }
            set
            {
                this.farelockExpirationDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FarelockPurchaseMessage
        {
            get
            {
                return this.farelockPurchaseMessage;
            }
            set
            {
                this.farelockPurchaseMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EarnedMilesHeader
        {
            get
            {
                return this.earnedMilesHeader;
            }
            set
            {
                this.earnedMilesHeader = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EarnedMilesText
        {
            get
            {
                return this.earnedMilesText;
            }
            set
            {
                this.earnedMilesText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string IneligibleToEarnCreditMessage
        {
            get
            {
                return this.ineligibleToEarnCreditMessage;
            }
            set
            {
                this.ineligibleToEarnCreditMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OaIneligibleToEarnCreditMessage
        {
            get
            {
                return this.oaIneligibleToEarnCreditMessage;
            }
            set
            {
                this.oaIneligibleToEarnCreditMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool AwardTravel
        {
            get
            {
                return this.awardTravel;
            }
            set
            {
                this.awardTravel = value;
            }
        }

        public bool PsSaTravel
        {
            get
            {
                return this.psSaTravel;
            }
            set
            {
                this.psSaTravel = value;
            }
        }

        public bool SupressLMX
        {
            get
            {
                return this.supressLMX;
            }
            set
            {
                this.supressLMX = value;
            }
        }

        public string OverMileageLimitMessage
        {
            get
            {
                return this.overMileageLimitMessage;
            }
            set
            {
                this.overMileageLimitMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OverMileageLimitAmount
        {
            get
            {
                return this.overMileageLimitAmount;
            }
            set
            {
                this.overMileageLimitAmount = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string syncedWithConcur = string.Empty;
        public string SyncedWithConcur
        {
            get
            {
                return this.syncedWithConcur;
            }
            set
            {
                this.syncedWithConcur = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TripType
        {
            get { return tripType; }
            set { this.tripType = value; }
        }

        public bool IsTPIIncluded
        {
            get
            {
                return this.isTPIIncluded;
            }
            set
            {
                this.isTPIIncluded = value;
            }
        }

        public bool IsFareLockOrNRSA
        {
            get
            {
                return this.isFareLockOrNRSA;
            }
            set
            {
                this.isFareLockOrNRSA = value;
            }
        }

        public bool HasCheckedBags
        {
            get
            {
                return this.hasCheckedBags;
            }
            set
            {
                this.hasCheckedBags = value;
            }
        }

        private List<MOBItem> urlItems = new List<MOBItem>();

        public List<MOBItem> URLItems
        {
            get
            {
                return urlItems;
            }
            set { urlItems = value; }
        }

        private List<MOBItem> editTravelerInfo;


        public List<MOBItem> EditTravelerInfo
        {
            get
            {
                return editTravelerInfo;
            }
            set { editTravelerInfo = value; }
        }

        private string encryptPNR;
        public string EncryptPNR
        {
            get { return encryptPNR; }
            set { encryptPNR = value; }
        }

        private string encryptLastName;
        public string EncryptLastName
        {
            get { return encryptLastName; }
            set { encryptLastName = value; }
        }

        private bool isgroup = false;
        public bool IsGroup
        {
            get { return isgroup; }
            set { isgroup = value; }
        }

        private bool isBulk = false;
        public bool IsBulk
        {
            get { return isBulk; }
            set { isBulk = value; }
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

        public bool HasScheduleChanged
        {
            get { return hasScheduleChanged; }
            set { hasScheduleChanged = value; }
        }
        public List<MOBItem> StatusMessageItems
        {
            get { return this.statusMessageItems; }
            set { this.statusMessageItems = value; }
        }


        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }
        public string ProductCategory
        {
            get { return productCategory; }
            set { productCategory = value; }
        }

        public string MarketType
        {
            get { return marketType; }
            set { marketType = value; }
        }

        public string CheckInStatus
        {
            get
            {
                return this.checkInStatus;
            }
            set
            {
                this.checkInStatus = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool GetCheckInStatusFromCSLPNRRetrivalService
        {
            get
            {
                return this.getCheckInStatusFromCSLPNRRetrivalService;
            }
            set
            {
                this.getCheckInStatusFromCSLPNRRetrivalService = value;
            }
        }

        public bool IrrOps
        {
            get
            {
                return this.irrOps;
            }
            set
            {
                this.irrOps = value;
            }
        }

        public bool IrrOpsViewed
        {
            get
            {
                return this.irrOpsViewed;
            }
            set
            {
                this.irrOpsViewed = value;
            }
        }

        private string fareLockMessage = string.Empty;
        private string fareLockPurchaseButton = string.Empty;
        private string fareLockPriceButton = string.Empty;

        public string FareLockMessage
        {
            get
            {
                return this.fareLockMessage;
            }
            set
            {
                this.fareLockMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FareLockPurchaseButton
        {
            get
            {
                return this.fareLockPurchaseButton;
            }
            set
            {
                this.fareLockPurchaseButton = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string FareLockPriceButton
        {
            get
            {
                return this.fareLockPriceButton;
            }
            set
            {
                this.fareLockPriceButton = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string AssociateMPId
        {
            get { return this.associateMPId; }
            set { this.associateMPId = value; }
        }

        public string AssociateMPIdSharesPosition
        {
            get { return this.associateMPIdSharesPosition; }
            set { this.associateMPIdSharesPosition = value; }
        }
        public string AssociateMPIdSharesGivenName
        {
            get { return this.associateMPIdSharesGivenName; }
            set { this.associateMPIdSharesGivenName = value; }
        }
        public string AssociateMPIdMessage
        {
            get { return this.associateMPIdMessage; }
            set { this.associateMPIdMessage = value; }
        }

        public List<MOBReservationPrice> Prices
        {
            get { return this.prices; }
            set { this.prices = value; }
        }

        public MOBOneClickEnrollmentEligibility OneClickEnrollmentEligibility
        {

            get { return this.oneClickEnrollmentEligibility; }
            set { this.oneClickEnrollmentEligibility = value; }
        }

    }
}
