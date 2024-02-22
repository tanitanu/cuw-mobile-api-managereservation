namespace United.Mobile.Model.Common
{
    //[Serializable()]
    //public class Reservation : IPersist
    //{
    //    #region IPersist Members

    //    private string objectName = "United.Persist.Definition.Shopping.Reservation";
    //    public string ObjectName
    //    {
    //        get
    //        {
    //            return this.objectName;
    //        }
    //        set
    //        {
    //            this.objectName = value;
    //        }
    //    }

    //    #endregion

    //    public string SessionId { get; set; }
    //    public string CartId { get; set; }
    //    public string MetaSessionId { get; set; }
    //    public bool IsSignedInWithMP { get; set; }
    //    public List<MOBSHOPTrip> Trips { get; set; }
    //    public List<MOBSHOPPrice> Prices { get; set; }
    //    public List<MOBSHOPTax> Taxes { get; set; }
    //    public List<string> TravelerKeys { get; set; }
    //    public SerializableDictionary<string, MOBSHOPTraveler> Travelers { get; set; }
    //    public SerializableDictionary<string, MOBCPTraveler> TravelersCSL { get; set; }
    //    public List<MOBSeatPrice> SeatPrices { get; set; }
    //    public List<MOBCreditCard> CreditCards { get; set; }
    //    public string CSLReservationJSONFormat { get; set; }
    //    public MOBSHOPRegisterOfferRequest MOBSHOPRegisterOfferRequest { get; set; }
    //    public List<MOBSHOPTravelOption> TravelOptions { get; set; }
    //    public MOBSHOPClubPassPurchaseRequest ClubPassPurchaseRequest { get; set; }
    //    public int NumberOfTravelers { get; set; }
    //    public bool TravelersRegistered { get; set; }
    //    public string SearchType = string.Empty;
    //    public List<Address> CreditCardsAddress { get; set; }
    //    public List<MOBSHOPFareRules> FareRules { get; set; }

    //    public MOBCPPhone ReservationPhone { get; set; }
    //    public MOBEmail ReservationEmail { get; set; }

    //    private string epaMessageTitle = string.Empty;
    //    private string epaMessage = string.Empty;

    //    private Boolean override24HrFlex;

    //    public Boolean Override24HrFlex { get { return this.override24HrFlex; } set { this.override24HrFlex = value; } }
    //    public string EPAMessageTitle { get; set; }
    //    public string EPAMessage { get; set; }

    //    private List<MOBLMXTraveler> lmxtravelers;
    //    public List<MOBLMXTraveler> LMXTravelers
    //    {
    //        get
    //        {
    //            return this.lmxtravelers;
    //        }
    //        set
    //        {
    //            this.lmxtravelers = value;
    //        }
    //    }

    //    private string pointOfSale = string.Empty;
    //    public string PointOfSale
    //    {
    //        get { return this.pointOfSale; }
    //        set { this.pointOfSale = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
    //    }

    //    private MOBSHOPFareLock fareLock;
    //    public MOBSHOPFareLock FareLock
    //    {
    //        get { return this.fareLock; }
    //        set { this.fareLock = value; }
    //    }

    //    private bool unregisterFareLock;
    //    public bool UnregisterFareLock
    //    {
    //        get { return this.unregisterFareLock; }
    //        set { this.unregisterFareLock = value; }
    //    }

    //    private bool awardTravel = false;
    //    public bool AwardTravel
    //    {
    //        get
    //        {
    //            return this.awardTravel;
    //        }
    //        set
    //        {
    //            this.awardTravel = value;
    //        }
    //    }

    //    public string FlightShareMessage = string.Empty;
    //    public string PKDispenserPublicKey = string.Empty;

    //    List<MOBLmxFlight> lmxFlights;
    //    public List<MOBLmxFlight> LMXFlights
    //    {
    //        get
    //        {
    //            return this.lmxFlights;
    //        }
    //        set
    //        {
    //            this.lmxFlights = value;
    //        }
    //    }

    //    private List<MOBItem> tcdAdvisoryMessages;
    //    public List<MOBItem> TCDAdvisoryMessages
    //    {
    //        get
    //        {
    //            return this.tcdAdvisoryMessages;
    //        }
    //        set
    //        {
    //            this.tcdAdvisoryMessages = value;
    //        }
    //    }

    //    private bool isRefundable;
    //    public bool IsRefundable
    //    {
    //        get
    //        {
    //            return this.isRefundable;
    //        }
    //        set
    //        {
    //            this.isRefundable = value;
    //        }
    //    }

    //    private bool isInternational;
    //    public bool ISInternational
    //    {
    //        get
    //        {
    //            return this.isInternational;
    //        }
    //        set
    //        {
    //            this.isInternational = value;
    //        }
    //    }


    //    private string ineligibleToEarnCreditMessage = string.Empty;
    //    public string IneligibleToEarnCreditMessage
    //    {
    //        get
    //        {
    //            return this.ineligibleToEarnCreditMessage;
    //        }
    //        set
    //        {
    //            this.ineligibleToEarnCreditMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
    //        }
    //    }

    //    private string oaIneligibleToEarnCreditMessage = string.Empty;
    //    public string OaIneligibleToEarnCreditMessage
    //    {
    //        get
    //        {
    //            return this.oaIneligibleToEarnCreditMessage;
    //        }
    //        set
    //        {
    //            this.oaIneligibleToEarnCreditMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
    //        }
    //    }

    //    private bool isFlexibleSegmentExist;
    //    public bool ISFlexibleSegmentExist
    //    {
    //        get
    //        {
    //            return this.isFlexibleSegmentExist;
    //        }
    //        set
    //        {
    //            this.isFlexibleSegmentExist = value;
    //        }
    //    }
    //    private bool getALLSavedTravelers = true;
    //    public bool GetALLSavedTravelers
    //    {
    //        get
    //        {
    //            return this.getALLSavedTravelers;
    //        }
    //        set
    //        {
    //            this.getALLSavedTravelers = value;
    //        }
    //    }
    //    private bool isGetProfileCalledForProfileOwner = false;

    //    public bool IsELF { get; set; }
    //    public bool IsMetaSearch { get; set; }

    //    public string ElfUpgradeMessagesForMetaSearch { get; set; }

    //    public bool IsUpgradedFromEntryLevelFare { get; set; }


    //    public bool IsGetProfileCalledForProfileOwner
    //    {
    //        get
    //        {
    //            return this.isGetProfileCalledForProfileOwner;
    //        }
    //        set
    //        {
    //            this.isGetProfileCalledForProfileOwner = value;
    //        }
    //    }

    //    private bool isCubaTravel;

    //    public bool IsCubaTravel
    //    {
    //        get { return isCubaTravel; }
    //        set { isCubaTravel = value; }
    //    }

    //    private MOBCPCubaTravel cubaTravelInfo;

    //    public MOBCPCubaTravel CubaTravelInfo
    //    {
    //        get { return cubaTravelInfo; }
    //        set { cubaTravelInfo = value; }
    //    }

    //    private MOBFormofPayment formOfPaymentType;

    //    public MOBFormofPayment FormOfPaymentType
    //    {
    //        get { return formOfPaymentType; }
    //        set { formOfPaymentType = value; }
    //    }

    //    private MOBPayPal payPal;

    //    public MOBPayPal PayPal
    //    {
    //        get { return payPal; }
    //        set { payPal = value; }
    //    }

    //    private MOBPayPalPayor payPalPayor;
    //    public MOBPayPalPayor PayPalPayor
    //    {
    //        get { return payPalPayor; }
    //        set { payPalPayor = value; }
    //    }

    //    private List<MOBTypeOption> fopOptions;
    //    public List<MOBTypeOption> FOPOptions
    //    {
    //        get
    //        {
    //            return this.fopOptions;
    //        }
    //        set { this.fopOptions = value; }
    //    }

    //    private List<MOBItem> elfMessagesForRTI;
    //    public List<MOBItem> ELFMessagesForRTI
    //    {
    //        get { return elfMessagesForRTI; }
    //        set { elfMessagesForRTI = value; }
    //    }


    //    public static implicit operator Service.Presentation.ReservationModel.Reservation(Reservation v)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    private MOBMasterpassSessionDetails masterpassSessionDetails;
    //    public MOBMasterpassSessionDetails MasterpassSessionDetails
    //    {
    //        get { return masterpassSessionDetails; }
    //        set { masterpassSessionDetails = value; }
    //    }

    //    private MOBMasterpass masterpass;
    //    public MOBMasterpass Masterpass
    //    {
    //        get { return masterpass; }
    //        set { masterpass = value; }
    //    }

    //    private bool isReshopChange;
    //    public bool IsReshopChange
    //    {
    //        get { return isReshopChange; }
    //        set { isReshopChange = value; }
    //    }

    //    private List<MOBSHOPReshopTrip> rehopTrips;
    //    public List<MOBSHOPReshopTrip> ReshopTrips
    //    {
    //        get { return rehopTrips; }
    //        set { rehopTrips = value; }
    //    }

    //    private List<MOBSHOPReshopTrip> reshopUsedTrips;
    //    public List<MOBSHOPReshopTrip> ReshopUsedTrips
    //    {
    //        get { return reshopUsedTrips; }
    //        set { reshopUsedTrips = value; }
    //    }

    //    private MOBSHOPReshop reshop;
    //    public MOBSHOPReshop Reshop
    //    {
    //        get { return reshop; }
    //        set { reshop = value; }
    //    }

    //    private bool isSSA;

    //    public bool IsSSA
    //    {
    //        get { return isSSA; }
    //        set { isSSA = value; }
    //    }

    //    private List<MOBItem> seatAssignmentMessage;

    //    public List<MOBItem> SeatAssignmentMessage
    //    {
    //        get { return seatAssignmentMessage; }
    //        set { seatAssignmentMessage = value; }
    //    }
    //    private MOBSHOPReservationInfo shopReservationInfo;
    //    public MOBSHOPReservationInfo ShopReservationInfo
    //    {
    //        get { return shopReservationInfo; }
    //        set { shopReservationInfo = value; }
    //    }

    //    private int aboveGoldMembers;

    //    public int AboveGoldMembers
    //    {
    //        get { return aboveGoldMembers; }
    //        set { aboveGoldMembers = value; }
    //    }

    //    private int goldMembers;

    //    public int GoldMembers
    //    {
    //        get { return goldMembers; }
    //        set { goldMembers = value; }
    //    }
    //    private int silverMembers;

    //    public int SilverMembers
    //    {
    //        get { return silverMembers; }
    //        set { silverMembers = value; }
    //    }

    //    private int noOfFreeEplusWithSubscriptions;

    //    public int NoOfFreeEplusWithSubscriptions
    //    {
    //        get { return noOfFreeEplusWithSubscriptions; }
    //        set { noOfFreeEplusWithSubscriptions = value; }
    //    }

    //    private MOBTripInsuranceFile tripInsuranceFile;
    //    public MOBTripInsuranceFile TripInsuranceFile
    //    {
    //        get { return this.tripInsuranceFile; }
    //        set { this.tripInsuranceFile = value; }
    //    }
    //    private MOBSHOPReservationInfo2 shopReservationInfo2;
    //    public MOBSHOPReservationInfo2 ShopReservationInfo2
    //    {
    //        get { return shopReservationInfo2; }
    //        set { shopReservationInfo2 = value; }
    //    }
    //    private List<MOBSection> alertMessages;
    //    public List<MOBSection> AlertMessages
    //    {
    //        get { return this.alertMessages; }
    //        set { this.alertMessages = value; }
    //    }
    //    private bool isRedirectToSecondaryPayment = false;
    //    public bool IsRedirectToSecondaryPayment
    //    {
    //        get
    //        {
    //            return this.isRedirectToSecondaryPayment;
    //        }
    //        set
    //        {
    //            this.isRedirectToSecondaryPayment = value;
    //        }
    //    }

    //    private string recordLocator = string.Empty;
    //    public string RecordLocator
    //    {
    //        get
    //        {
    //            return this.recordLocator;
    //        }
    //        set
    //        {
    //            this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
    //        }
    //    }

    //    private List<string> messages;
    //    public List<string> Messages
    //    {
    //        get
    //        {
    //            return this.messages;
    //        }
    //        set
    //        {
    //            this.messages = value;
    //        }
    //    }

    //    private string checkedbagChargebutton;

    //    public string CheckedbagChargebutton
    //    {
    //        get { return checkedbagChargebutton; }
    //        set { checkedbagChargebutton = value; }
    //    }

    //    private bool isBookingCommonFOPEnabled;

    //    public bool IsBookingCommonFOPEnabled
    //    {
    //        get { return isBookingCommonFOPEnabled; }
    //        set { isBookingCommonFOPEnabled = value; }
    //    }

    //    private bool isReshopCommonFOPEnabled;

    //    public bool IsReshopCommonFOPEnabled
    //    {
    //        get { return isReshopCommonFOPEnabled; }
    //        set { isReshopCommonFOPEnabled = value; }
    //    }
    //    private bool isPostBookingCommonFOPEnabled;

    //    public bool IsPostBookingCommonFOPEnabled
    //    {
    //        get { return isPostBookingCommonFOPEnabled; }
    //        set { isPostBookingCommonFOPEnabled = value; }
    //    }
    //    public string RecLoc { get; set; } = string.Empty;
    //    public string RecLocCrtDt { get; set; } = string.Empty;
    //    public string NickName { get; set; } = string.Empty;
    //    [System.Xml.Serialization.XmlElementAttribute("Segment")]
    //    public Segment[] Segment { get; set; }
    //    [System.Xml.Serialization.XmlElementAttribute("Traveller")]
    //    public Traveller[] Traveller { get; set; }
    //    [System.Xml.Serialization.XmlAttributeAttribute()]
    //    public bool hasFFC { get; set; }
    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute()]
    //    public bool isCancelled { get; set; }
    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute()]
    //    public bool isFarelockPNR { get; set; }
    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlAttributeAttribute()]
    //    public string FFCExpirationDt { get; set; } = string.Empty;
    //}
}
