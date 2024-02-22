using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBSHOPAvailability
    {
        private string sessionId = string.Empty;
        private string cartId = string.Empty;
        private decimal closeInBookingFee;
        private List<MOBSHOPFareWheelItem> fareWheel;
        private MOBSHOPTrip trip;
        private MOBSHOPReservation reservation;
        private FareLock fareLock;
        //private List<MOBSHOPOffer> fareLock;
        private SHOPAwardCalendar awardCalendar;
        private AwardDynamicCalendar awardDynamicCalendar;

        private string callDuration = string.Empty;
        private string uaDiscount = string.Empty;
        private bool awardTravel = false;
        private List<MOBItem> elfShopMessages;
        private List<Option> elfShopOptions;
        private bool offerMetaSearchElfUpsell = false;
        private List<UpsellProduct> upsells;
        private string corporateDisclaimer;
        private bool disablePricingBySlice;
        private string priceTextDescription = string.Empty;
        private string sortDisclaimerText = string.Empty;
        private List<MOBFSRAlertMessage> fsrAlertMessages;
        private string availableAwardMiles;
        private string availableAwardMilesWithDesc;
        private int travelerCount;
        private List<InfoWarningMessages> infoMessages;
        private string title;
        private string subTitle;
        private bool isShopRefreshRequired;
        private string responseType;
        private MOBNoFlightFoundMessage noFlightFoundMessage;
        private MOBSection promoCodeRemoveAlertForProducts;
        private bool hideFareWheel = false;
        private int maxTPFlightSelectCount;
        private string tripPlanId;
        private string tripPlanCartId;
        private string tpNoAvailabilityMsg;
        private string tpSelectAnotherFlightLinkTxt;
        private string travelType;
        private List<MOBMobileCMSContentMessages> contentMessages;
        private List<MOBFareComparison> fareComparisonMessage;
        private TravelPolicy corporateOutOfPolicy;

        public TravelPolicy CorporateOutOfPolicy
        {
            get { return corporateOutOfPolicy; }
            set { corporateOutOfPolicy = value; }
        }

        public List<MOBFareComparison> FareComparisonMessage
        {
            get
            {
                return this.fareComparisonMessage;
            }
            set
            {
                this.fareComparisonMessage = value;
            }
        }

        public List<MOBMobileCMSContentMessages> ContentMessages
        {
            get { return contentMessages; }
            set { contentMessages = value; }
        }
        public string TravelType
        {
            get { return travelType; }
            set { travelType = value; }
        }

        public string TpNoAvailabilityMsg
        {
            get { return tpNoAvailabilityMsg; }
            set { tpNoAvailabilityMsg = value; }
        }
        public string TpSelectAnotherFlightLinkTxt
        {
            get { return tpSelectAnotherFlightLinkTxt; }
            set { tpSelectAnotherFlightLinkTxt = value; }
        }
        public string TripPlanCartId
        {
            get { return tripPlanCartId; }
            set { tripPlanCartId = value; }
        }
        public string TripPlanId
        {
            get { return tripPlanId; }
            set { tripPlanId = value; }
        }
        public int MaxTPFlightSelectCount
        {
            get { return maxTPFlightSelectCount; }
            set { maxTPFlightSelectCount = value; }
        }

        public bool HideFareWheel
        {
            get { return hideFareWheel; }
            set { hideFareWheel = value; }
        }

        public MOBSection PromoCodeRemoveAlertForProducts
        {
            get { return promoCodeRemoveAlertForProducts; }
            set { promoCodeRemoveAlertForProducts = value; }
        }

        public List<InfoWarningMessages> InfoMessages
        {
            get { return infoMessages; }
            set { infoMessages = value; }
        }

        public int TravelerCount
        {
            get
            {
                return travelerCount;
            }
            set
            {
                travelerCount = value;
            }
        }

        public string CorporateDisclaimer
        {
            get { return corporateDisclaimer; }
            set
            {
                corporateDisclaimer = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string CartId
        {
            get
            {
                return this.cartId;
            }
            set
            {
                this.cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public decimal CloseInBookingFee
        {
            get
            {
                return this.closeInBookingFee;
            }
            set
            {
                this.closeInBookingFee = value;
            }
        }

        public List<MOBSHOPFareWheelItem> FareWheel
        {
            get
            {
                return this.fareWheel;
            }
            set
            {
                this.fareWheel = value;
            }
        }


        public MOBSHOPTrip Trip
        {
            get
            {
                return this.trip;
            }
            set
            {
                this.trip = value;
            }
        }

        public MOBSHOPReservation Reservation
        {
            get
            {
                return this.reservation;
            }
            set
            {
                this.reservation = value;
            }
        }

        public FareLock FareLock
        {
            get
            {
                return this.fareLock;
            }
            set
            {
                this.fareLock = value;
            }
        }

        //public List<MOBSHOPOffer> FareLock
        //{
        //    get
        //    {
        //        return this.fareLock;
        //    }
        //    set
        //    {
        //        this.fareLock = value;
        //    }
        //}

        public SHOPAwardCalendar AwardCalendar
        {
            get
            {
                return this.awardCalendar;
            }
            set
            {
                this.awardCalendar = value;
            }
        }
        public AwardDynamicCalendar AwardDynamicCalendar
        {
            get { return this.awardDynamicCalendar; }
            set
            {
                this.awardDynamicCalendar = value;
            }
        }
        public string CallDuration
        {
            get
            {
                return this.callDuration;
            }
            set
            {
                this.callDuration = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string UaDiscount
        {
            get
            {
                return this.uaDiscount;
            }
            set
            {
                this.uaDiscount = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBItem> ELFShopMessages
        {
            get
            {
                return this.elfShopMessages;
            }
            set
            {
                this.elfShopMessages = value;
            }
        }

        [XmlArrayItem("MOBSHOPOption")]
        public List<Option> ELFShopOptions
        {
            get
            {
                return this.elfShopOptions;
            }
            set
            {
                this.elfShopOptions = value;
            }
        }

        public bool OfferMetaSearchElfUpsell
        {
            get
            {
                return this.offerMetaSearchElfUpsell;
            }
            set
            {
                this.offerMetaSearchElfUpsell = value;
            }
        }

        public List<UpsellProduct> Upsells
        {
            get { return this.upsells; }
            set { this.upsells = value; }
        }
        public bool DisablePricingBySlice
        {
            get
            {
                return this.disablePricingBySlice;
            }
            set
            {
                this.disablePricingBySlice = value;
            }
        }
        public string PriceTextDescription
        {
            get { return priceTextDescription; }
            set { priceTextDescription = value; }
        }
        public string fSRFareDescription { get; set; } = string.Empty;

        public string SortDisclaimerText
        {
            get { return sortDisclaimerText; }
            set { sortDisclaimerText = value; }
        }

        public List<MOBFSRAlertMessage> FSRAlertMessages
        {
            get { return fsrAlertMessages; }
            set { fsrAlertMessages = value; }
        }
        public string AvailableAwardMiles
        {
            get { return availableAwardMiles; }
            set { availableAwardMiles = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
        public string AvailableAwardMilesWithDesc
        {
            get { return availableAwardMilesWithDesc; }
            set { availableAwardMilesWithDesc = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Title
        {
            get { return title; }
            set { title = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string SubTitle
        {
            get { return subTitle; }
            set { subTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public bool IsShopRefreshRequired
        {
            get { return isShopRefreshRequired; }
            set { isShopRefreshRequired = value; }
        }
        public string ResponseType
        {
            get { return responseType; }
            set { responseType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public MOBNoFlightFoundMessage NoFlightFoundMessage
        {
            get { return noFlightFoundMessage; }
            set { noFlightFoundMessage = value; }
        }
        public MOBSHOPAvailability()
        {
            //ELFShopMessages = new List<MOBItem>();
            //ELFShopOptions = new List<Option>();
        }
    }
   

    
}
