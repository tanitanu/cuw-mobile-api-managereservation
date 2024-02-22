using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping.Misc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class MOBInFlightMealsRefreshmentsRequest: MOBRequest
    {
        private string sessionId;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        private string segmentId   ;

        public string SegmentId
        {
            get { return segmentId; }
            set { segmentId = value; }
        }

        private string passengerId;

        public string PassengerId
        {
            get { return passengerId; }
            set { passengerId = value; }
        }

        private List<MOBInFlightMealSelectedRefreshments> selectedRefreshments;

        public List<MOBInFlightMealSelectedRefreshments> SelectedRefreshments
        {
            get { return selectedRefreshments; }
            set { selectedRefreshments = value; }
        }

        private InflightMealRefreshmentsActionType inflightMealRefreshmentsActionType;

        public InflightMealRefreshmentsActionType InflightMealRefreshmentsActionType
        {
            get { return inflightMealRefreshmentsActionType; }
            set { inflightMealRefreshmentsActionType = value; }
        }

        private string productCode;

        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }

        private string encryptedQueryString;

        public string EncryptedQueryString
        {
            get { return encryptedQueryString; }
            set { encryptedQueryString = value; }
        }

        private string premiumCabinMealEmailAddress;

        public string PremiumCabinMealEmailAddress
        {
            get { return premiumCabinMealEmailAddress; }
            set { premiumCabinMealEmailAddress = value; }
        }

        private string recordLocator;

        public string RecordLocator
        {
            get { return recordLocator; }
            set { recordLocator = value; }
        }


        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private List<MOBItem> catalogValues;

        public List<MOBItem> CatalogValues
        {
            get
            {
                return this.catalogValues;
            }
            set
            {
                this.catalogValues = value;
            }
        }
    }

    [Serializable]
    public class MOBInFlightMealsCompletePurchaseOrCancelRequest : MOBRequest
    {
        private string sessionId;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        private string segmentId;

        public string SegmentId
        {
            get { return segmentId; }
            set { segmentId = value; }
        }

        private string passengerId;

        public string PassengerId
        {
            get { return passengerId; }
            set { passengerId = value; }
        }

        private List<MOBInFlightMealSelectedRefreshments> selectedRefreshments;

        public List<MOBInFlightMealSelectedRefreshments> SelectedRefreshments
        {
            get { return selectedRefreshments; }
            set { selectedRefreshments = value; }
        }

        private InflightMealRefreshmentsActionType inflightMealRefreshmentsActionType;

        public InflightMealRefreshmentsActionType InflightMealRefreshmentsActionType
        {
            get { return inflightMealRefreshmentsActionType; }
            set { inflightMealRefreshmentsActionType = value; }
        }



    }

    [Serializable]
    public class MOBInFlightMealsCompletePurchaseOrCancelResponse : MOBResponse
    {
        private List<MOBItem> captions;

        public List<MOBItem> Captions
        {
            get { return captions; }
            set { captions = value; }
        }

        private List<MOBInflightMealSegment> flightSegments;

        public List<MOBInflightMealSegment> FlightSegments
        {
            get { return flightSegments; }
            set { flightSegments = value; }
        }

        private string sessionId = string.Empty;


        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

    }

    [Serializable]
    public class MOBInFlightMealsRefreshmentsCheckoutRequest : MOBRequest
    {
        private string sessionId;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }


        private List<MOBInFlightMealSelectedRefreshments> selectedRefreshments;

        public List<MOBInFlightMealSelectedRefreshments> SelectedRefreshments
        {
            get { return selectedRefreshments; }
            set { selectedRefreshments = value; }
        }

    }

    public class MOBInflightMeals
    {
        private string passengerFullName;

        public string PassengerFullName
        {
            get { return passengerFullName; }
            set { passengerFullName = value; }
        }

        // (Pringles*1, $7.98)
        private List<MOBInflightMealSummary> summaryOfPurchases;

        public List<MOBInflightMealSummary> SummaryOfPurchases
        {
            get { return summaryOfPurchases; }
            set { summaryOfPurchases = value; }
        }

        private string totalDisplayPrice;

        public string TotalDisplayPrice
        {
            get { return totalDisplayPrice; }
            set { totalDisplayPrice = value; }
        }

    }

    [Serializable]
    public class MOBInFlightMealsRefreshmentsCheckoutRespose : MOBResponse
    {



        // (Pringles*1, $7.98)
        private List<MOBInflightMealSummary> summaryOfPurchases;

        public List<MOBInflightMealSummary> SummaryOfPurchases
        {
            get { return summaryOfPurchases; }
            set { summaryOfPurchases = value; }
        }
    }


   

    [Serializable]
    public class MOBInFlightMealSelectedRefreshments
    {

       
        private string selectedMealCode ;

        public string SelectedMealCode
        {
            get { return selectedMealCode; }
            set { selectedMealCode = value; }
        }

        private string selectedProductId;

        public string SelectedProductId
        {
            get { return selectedProductId; }
            set { selectedProductId = value; }
        }

        private int quantity;

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        private string selectedMealType;

        public string SelectedMealType
        {
            get { return selectedMealType; }
            set { selectedMealType = value; }
        }

    }

    [Serializable]
    public class MOBInFlightMealsOfferRequest : MOBRequest
    {
        private string sessionId = string.Empty;
       

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        private InflightMealOffersFlowType inflightMealOffersActionType;

        public InflightMealOffersFlowType InflightMealOffersActionType
        {
            get { return inflightMealOffersActionType; }
            set { inflightMealOffersActionType = value; }
        }

        private string productCode;

        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }
        //Deeplink
        private string encryptedQueryString;

        public string EncryptedQueryString
        {
            get { return encryptedQueryString; }
            set { encryptedQueryString = value; }
        }

        // push notification
        private string recordLocator;

        public string RecordLocator
        {
            get { return recordLocator; }
            set { recordLocator = value; }
        }


        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private List<MOBItem> catalogValues;

        public List<MOBItem> CatalogValues
        {
            get
            {
                return this.catalogValues;
            }
            set
            {
                this.catalogValues = value;
            }
        }

    }
    [Serializable]
    public class MOBInFlightMealsOfferResponse : MOBResponse
    {
        [System.Text.Json.Serialization.JsonIgnore()]
        public string ObjectName { get; set; } = "United.Definition.PreOrderMeals.MOBInFlightMealsOfferResponse";
        private List<MOBItem> captions;

        public List<MOBItem> Captions
        {
            get { return captions; }
            set { captions = value; }
        }

        private List<MOBInflightMealSegment> flightSegments;

        public List<MOBInflightMealSegment> FlightSegments
        {
            get { return flightSegments; }
            set { flightSegments = value; }
        }

        private string sessionId = string.Empty;


        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        private bool enableBackButton;

        public bool EnableBackButton
        {
            get { return enableBackButton; }
            set { enableBackButton = value; }
        }

        private bool enableReservationDetailButton;

        public bool EnableReservationDetailButton
        {
            get { return enableReservationDetailButton; }
            set { enableReservationDetailButton = value; }
        }

        private string productCode;

        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }

        private List<Section> alertMessages;

        public List<Section> AlertMessages
        {
            get { return alertMessages; }
            set { alertMessages = value; }
        }
        // Deeplink
        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private string recordLocator;

        public string RecordLocator
        {
            get { return recordLocator; }
            set { recordLocator = value; }
        }

        private List<MOBMobileCMSContentMessages> termsNConditions;

        public List<MOBMobileCMSContentMessages> TermsNConditions
        {
            get { return termsNConditions; }
            set { termsNConditions = value; }
        }

        // Value will be 0 or 5 
        private InflightMealRefreshmentsActionType inflightMealAddOrEditFlow;

        public InflightMealRefreshmentsActionType InflightMealAddOrEditFlow
        {
            get { return inflightMealAddOrEditFlow; }
            set { inflightMealAddOrEditFlow = value; }
        }
    }

    [Serializable]
    public class MOBInFlightMealsRefreshmentsResponse : MOBResponse
    {
        [System.Text.Json.Serialization.JsonIgnore()]
        public string ObjectName { get; set; } = "United.Definition.PreOrderMeals.MOBInFlightMealsRefreshmentsResponse";
        private List<MOBItem> captions;

        public List<MOBItem> Captions
        {
            get { return captions; }
            set { captions = value; }
        }

        private MOBInFlightMealPassenger passenger;

        public MOBInFlightMealPassenger Passenger
        {
            get { return passenger; }
            set { passenger = value; }
        }

        
        private List<MOBItem> snackDietIndicators;

        public List<MOBItem> SnackDietIndicators
        {
            get { return snackDietIndicators; }
            set { snackDietIndicators = value; }
        }

        private List<MOBItem> freeMealsDietIndicators;

        public List<MOBItem> FreeMealsDietIndicators
        {
            get { return freeMealsDietIndicators; }
            set { freeMealsDietIndicators = value; }
        }

        private List<MOBItem> beverageDietIndicators;

        public List<MOBItem> BeverageDietIndicators
        {
            get { return beverageDietIndicators; }
            set { beverageDietIndicators = value; }
        }

        /* "Traveler 1 of 2 |  Jack Brinker"; */
        private string flightDescription;

        public string FlightDescription
        {
            get { return flightDescription; }
            set { flightDescription = value; }
        }


        // Below is required in the screen to select snacks
        private List<MOBInFlightMealRefreshment> snacks;

        public List<MOBInFlightMealRefreshment> Snacks
        {
            get { return snacks; }
            set { snacks = value; }
        }

        // Below is required in the screen to select beverages
        private List<MOBInFlightMealRefreshment> beverages;

        public List<MOBInFlightMealRefreshment> Beverages
        {
            get { return beverages; }
            set { beverages = value; }
        }

        // Below is required in the screen to select beverages
        private List<MOBInFlightMealRefreshment> freeMeals;

        public List<MOBInFlightMealRefreshment> FreeMeals
        {
            get { return freeMeals; }
            set { freeMeals = value; }
        }

        private List<MOBInFlightMealRefreshment> preArrivalFreeMeals;

        public List<MOBInFlightMealRefreshment> PreArrivalFreeMeals
        {
            get { return preArrivalFreeMeals; }
            set { preArrivalFreeMeals = value; }
        }


        // Below is required in the screen to select beverages
        private List<MOBInFlightMealRefreshment> specialMeals;

        public List<MOBInFlightMealRefreshment> SpecialMeals
        {
            get { return specialMeals; }
            set { specialMeals = value; }
        }

        private List<MOBInFlightMealRefreshment> differentPlanOptions;

        public List<MOBInFlightMealRefreshment> DifferentPlanOptions
        {
            get { return differentPlanOptions; }
            set { differentPlanOptions = value; }
        }

        private int maxQuantity;

        public int MaxQuantity
        {
            get { return maxQuantity; }
            set { maxQuantity = value; }
        }

      
        private string maxQuantityTxt;

        public string MaxQuantityTxt
        {
            get { return maxQuantityTxt; }
            set { maxQuantityTxt = value; }
        }


        private bool isPreviousTravellerAvailable;

        public bool IsPreviousTravellerAvailable
        {
            get { return isPreviousTravellerAvailable; }
            set { isPreviousTravellerAvailable = value; }
        }

        private bool isNextTravellerAvailable;

        public bool IsNextTravellerAvailable
        {
            get { return isNextTravellerAvailable; }
            set { isNextTravellerAvailable = value; }
        }

        private string sessionId;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        private string  segmentId;

        public string SegmentId
        {
            get { return segmentId; }
            set { segmentId = value; }
        }

        private string productCode;

        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }


        private List<MOBFSRAlertMessage> alertMessages;

        public List<MOBFSRAlertMessage> AlertMessages
        {
            get { return alertMessages; }
            set { alertMessages = value; }
        }

        private string premiumCabinMealEmailAddress;

        public string PremiumCabinMealEmailAddress
        {
            get { return premiumCabinMealEmailAddress; }
            set { premiumCabinMealEmailAddress = value; }
        }

        private List<MOBInFlightMealRefreshment> preArrivalDifferentPlanOptions;

        public List<MOBInFlightMealRefreshment> PreArrivalDifferentPlanOptions
        {
            get { return preArrivalDifferentPlanOptions; }
            set { preArrivalDifferentPlanOptions = value; }
        }

        private bool isSaveBtnAllowedInEconomy;
        public bool IsSaveBtnAllowedInEconomy
        {
            get { return isSaveBtnAllowedInEconomy; }
            set { isSaveBtnAllowedInEconomy = value; }
        }

    }

   



    [Serializable]
    public class MOBInflightMealSegment
    {
        private string origin;

        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        private string destination;

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        private string snacksPurchaseLinkText;

        public string SnacksPurchaseLinkText
        {
            get { return snacksPurchaseLinkText; }
            set { snacksPurchaseLinkText = value; }
        }

        private string segmentId;

        public string SegmentId
        {
            get { return segmentId; }
            set { segmentId = value; }
        }

        private string tripNumber;

        public string TripNumber
        {
            get { return tripNumber; }
            set { tripNumber = value; }
        }


        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        private bool isMealSelctionAvailable;

        public bool IsMealSelctionAvailable
        {
            get { return isMealSelctionAvailable; }
            set { isMealSelctionAvailable = value; }
        }

        /* if IsMealSelctionAvailable == false , Display InEligibleMealText which is "Selection not available" */
        private string inEligibleMealText;

        public string InEligibleMealText 
        {
            get { return inEligibleMealText; }
            set { inEligibleMealText = value; }
        }


        private string  orderSummary;

        public string OrderSummary
        {
            get { return orderSummary; }
            set { orderSummary = value; }
        }


        private List<MOBInFlightMealPassenger> passengers;

        public List<MOBInFlightMealPassenger> Passengers
        {
            get { return passengers; }
            set { passengers = value; }
        }

        private string flightNumber;

        public string FlightNumber
        {
            get { return flightNumber; }
            set { flightNumber = value; }
        }

        private string departureDate;
        private string footer;
        public string DepartureDate
        {
            get { return departureDate; }
            set { departureDate = value; }
        }
        public string Footer { get => footer; set => footer = value; }





    }
    [Serializable]
    public class MOBInFlightMealPassenger
    {
        //private string firstName;

        //public string FirstName
        //{
        //    get { return firstName; }
        //    set { firstName = value; }
        //}

        //private string lastName;

        //public string LastName
        //{
        //    get { return lastName; }
        //    set { lastName = value; }
        //}

        private string fullName;

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        private string passengerId;

        public string PassengerId
        {
            get { return passengerId; }
            set { passengerId = value; }
        }


        /* "Traveler 2 of 2 | Jack Brinker"*/
        private string passengerDesc;

        public string PassengerDesc
        {
            get { return passengerDesc; }
            set { passengerDesc = value; }
        }


        // this property will be used in the PreOrder Refereshments screen
        private List<MOBItem> captions;

        public List<MOBItem> Captions
        {
            get { return captions; }
            set { captions = value; }
        }


        private string addSnackLinkText;

        [JsonProperty(PropertyName = "addSnackLinkText")]
        [JsonPropertyName("addSnackLinkText")]
        public string AddSnacksLinkText
        {
            get { return addSnackLinkText; }
            set { addSnackLinkText = value; }
        }

        /* '-' */
        private string notEligibileDefaultText;

        public string NotEligibileDefaultText
        {
            get { return notEligibileDefaultText; }
            set { notEligibileDefaultText = value; }
        }


        // (Pringles*1, $7.98)
        private List<MOBInflightMealSummary> summaryOfPurchases;

        public List<MOBInflightMealSummary> SummaryOfPurchases
        {
            get { return summaryOfPurchases; }
            set { summaryOfPurchases = value; }
        }

        /* The below 3 properties are required when there is no eligibility for bevereages*/
        private string beverageEligiblityDesc;

        public string BeverageEligiblityDesc
        {
            get { return beverageEligiblityDesc; }
            set { beverageEligiblityDesc = value; }
        }

        private bool isEligibleForBeverages;

        public bool IsEligibleForBeverages
        {
            get { return isEligibleForBeverages; }
            set { isEligibleForBeverages = value; }
        }

        private string softDrinksURL;

        public string SoftDrinksURL
        {
            get { return softDrinksURL; }
            set { softDrinksURL = value; }
        }

        //Dynamic PO< 

        private bool isEditable;

        public bool IsEditable
        {
            get { return isEditable; }
            set { isEditable = value; }
        }

        private string editText;

        public string EditText
        {
            get { return editText; }
            set { editText = value; }
        }

    }

    [Serializable]
    public class MOBInflightMealSummary
    {
        private string text;
        private string price;
        private string mealCode; 
        private bool isBold;

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }

        public string Price
        {
            get
            {
                return this.price;
            }
            set
            {
                this.price = value;
            }
        }
        public string MealCode
        {
            get
            {
                return this.mealCode;
            }
            set
            {
                this.mealCode = value;
            }
        }

        public bool IsBold
        {
            get
            {
                return this.isBold;
            }
            set
            {
                this.isBold = value;
            }
        }
    }


    [Serializable]
    public class MOBInFlightMealRefreshment
    {
        private string imageURL;
        private double price;
        private string displayPrice;
        private string mealCode;
        private string mealName;
        private string mealDescription;
        private int maxQty;
        private int offerQuantity;
        private string maxQtyTxt;
        private string mealServiceCode;
        private string isMealAvailable;
        private string notAvailableMessage;
        private int quantity;
        private string mealType;
        private string sequenceNumber;
        private string outOfStockText;

        public string ImageURL { get => imageURL; set => imageURL = value; }

        public double Price { get => price; set => price = value; }

        public string DisplayPrice { get => displayPrice; set => displayPrice = value; }

        public string MealCode { get => mealCode; set => mealCode = value; }

        public string MealName { get => mealName; set => mealName = value; }

        public string MealDescription { get => mealDescription; set => mealDescription = value; }


        public int MaxQty { get => maxQty; set => maxQty = value; }

        public int OfferQuantity { get => offerQuantity; set => offerQuantity = value; }

        public string MaxQtyTxt { get => maxQtyTxt; set => maxQtyTxt = value; }

        public string MealServiceCode { get => mealServiceCode; set => mealServiceCode = value; }

        public string IsMealAvailable { get => isMealAvailable; set => isMealAvailable = value; }

        public string NotAvailableMessage { get => notAvailableMessage; set => notAvailableMessage = value; }

        //selected quantity
        public int Quantity { get => quantity; set => quantity = value; }

        public string MealType { get => mealType; set => mealType = value; }

        public string SequenceNumber { get => sequenceNumber; set => sequenceNumber = value; }

        public string OutOfStockText { get => outOfStockText; set => outOfStockText = value; }


        private string productId;

        public string ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        private Collection<string> dietIndicators;

        public Collection<string> DietIndicators
        {
            get { return dietIndicators; }
            set { dietIndicators = value; }
        }

    }

    public enum InflightMealRefreshmentsActionType
    {
        TapOnAddSnacksLink = 0,// Add Snacks click on Landing screen
        TapOnNextTravelerButton = 1, // Next Traveler button on Snacks and Beverages screen
        TapOnPreviousTravelerButton = 2,//Previous Traveler button on Snacks and Beverages screen
        TapOnContinueCheckoutButton = 3, //Continue checkout on Snacks and Beverages screen first call after success, registeroffer call
        TapOnSaveNContinue = 4, // Save and Continue click for Upper cabin registeroffer
        TapOnEditLink = 5
    }

    public enum InflightMealOffersFlowType
    {
        TapOnManagerReservationScreen = 0,//Sequential call after PNRRecordLocator API call
        TapOnSnBCancelClick = 1,//Cancel click call of Snacks and Beverages screen
        TapOnAgreeNPurchaseClick = 2,// Agree n purchase
        TapOnSnBCloseButton = 3,//Cancel click call of Snacks and Beverages screen
        TapOnSaveNContinue = 4, // Save and Continue click for Upper cabin registeroffer
        TapOnDeeplinkClick = 5
    }

    public enum InflightMealOffersRefreshmentsDisplayOrder
    {
         FreeMeals = 0,
         Snacks = 1,
         Beverages = 2
    }

    public enum InflightRefreshementType
    {
        Breakfast = 0,
        Dinner = 1,
        Snack = 2,
        Beverage = 3,
        Lunch =4,
        Refreshment =5,
        SPML = 6, //special Meal
        NONMEAL = 7,
        Prearrival = 8
    }

    public enum InflightMealType
    {
        Refreshment = 0,
        Meal = 1,
        NONMEAL = 2,
        SPML = 3,
        Prearrival = 4
    }
}
