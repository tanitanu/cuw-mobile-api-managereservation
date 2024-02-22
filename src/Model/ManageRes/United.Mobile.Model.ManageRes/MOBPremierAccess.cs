using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Fitbit;

namespace United.Mobile.Model.ManageRes
{
    [Serializable]
    public class MOBPremierAccess
    {
        private string productCode = string.Empty;
        private string productName = string.Empty;
        private string recordLocator = string.Empty;
        private string lastName = string.Empty;
        private string paOffersSessionId = string.Empty;
        private List<MOBPASegment> segments = null;
        private List<MOBTypeOption> benefits = null;
        private List<MOBTypeOption> tAndC = null;
        private string individualFlights = string.Empty;
        private decimal paForCurrentTrip;
        private string currencyCode = string.Empty;
        private string discountedPercentage = string.Empty;
        private string errorMessage = string.Empty;
        private MOBPremierAccessOfferTileInfo paOfferTileInfo;
        private MOBPASummary paPaymentSummary;

        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
        public string RecordLocator
        {
            get { return recordLocator; }
            set { recordLocator = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string PAOffersSessionId
        {
            get
            {
                return this.paOffersSessionId;
            }
            set
            {
                this.paOffersSessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBPASegment> Segments
        {
            get { return segments; }
            set { segments = value; }
        }

        public List<MOBTypeOption> Benefits
        {
            get { return benefits; }
            set { benefits = value; }
        }

        public List<MOBTypeOption> TAndC
        {
            get { return tAndC; }
            set { tAndC = value; }
        }

        public string IndividualFlights
        {
            get { return individualFlights; }
            set { individualFlights = value; }
        }

        public decimal PAForCurrentTrip
        {
            get { return paForCurrentTrip; }
            set { paForCurrentTrip = value; }
        }

        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }

        public string DiscountedPercentage
        {
            get { return discountedPercentage; }
            set { discountedPercentage = value; }
        }

        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set { this.errorMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public MOBPremierAccessOfferTileInfo PAOfferTileInfo
        {
            get { return this.paOfferTileInfo; }
            set { this.paOfferTileInfo = value; }
        }

        public MOBPASummary PAPaymentSummary
        {
            get { return paPaymentSummary; }
            set { paPaymentSummary = value; }
        }
    }

    [Serializable]
    public class MOBPASegment
    {
        private string origin = string.Empty;
        private string destination = string.Empty;
        private string flightDate = string.Empty;
        private string flightNumber = string.Empty;
        private List<MOBPACustomer> customers = null;
        private MOBPASegmentType paSegmentType;
        private string segmentId = string.Empty;
        private string addAllTravelers = string.Empty;
        private bool priorityBoarding = false;
        private bool priorityCheckin = false;
        private bool prioritySecurity = false;
        private double discountedPrice = 0;


        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public string FlightDate
        {
            get { return flightDate; }
            set { flightDate = value; }
        }

        public string FlightNumber
        {
            get { return flightNumber; }
            set { flightNumber = value; }
        }

        public List<MOBPACustomer> Customers
        {
            get { return customers; }
            set { customers = value; }
        }

        public MOBPASegmentType PASegmentType
        {
            get { return paSegmentType; }
            set { paSegmentType = value; }
        }

        public string SegmentId
        {
            get { return segmentId; }
            set { segmentId = value; }
        }

        public string AddAllTravelers
        {
            get { return addAllTravelers; }
            set { addAllTravelers = value; }
        }


        public bool PriorityBoarding
        {
            get { return priorityBoarding; }
            set { priorityBoarding = value; }
        }


        public bool PriorityCheckin
        {
            get { return priorityCheckin; }
            set { priorityCheckin = value; }
        }


        public bool PrioritySecurity
        {
            get { return prioritySecurity; }
            set { prioritySecurity = value; }
        }

        public double DiscountedPrice
        {
            get { return discountedPrice; }
            set { discountedPrice = value; }
        }


    }

    [Serializable]
    public class MOBPACustomer
    {
        private string custId = string.Empty;
        private string custName = string.Empty;
        private decimal fee;
        private string currencyCode = string.Empty;
        public bool alreadypurchased { get; set; } = false;
        private MOBPACustomerType CustomerType;
        private List<string> productIds;
        private Int32 miles;
        private string displayMiles;

        public MOBPACustomer() { }

        public MOBPACustomer(string custId, string custName, string fee, bool alreadypurchased)
        {
            CustId = custId;

        }

        public string CustId
        {
            get { return custId; }
            set { custId = value; }
        }

        public string CustName
        {
            get { return custName; }
            set { custName = value; }
        }

        public decimal Fee
        {
            get { return fee; }
            set { fee = value; }
        }

        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }

        //public bool AlreadyPurchased
        //{
        //    get { return alreadypurchased; }
        //    set { alreadypurchased = value; }
        //}

        public MOBPACustomerType paCustomerType
        {
            get { return CustomerType; }
            set { CustomerType = value; }
        }
        public List<string> ProductIds
        {
            get { return productIds; }
            set { productIds = value; }
        }
        public Int32 Miles
        {
            get { return miles; }
            set { miles = value; }
        }
        public string DisplayMiles
        {
            get { return displayMiles; }
            set { displayMiles = value; }
        }
    }

    [Serializable]
    public class MOBPremierAccessOfferTileInfo
    {
        private string offerTitle = string.Empty;
        private decimal price;
        private string currencyCode = string.Empty;
        private string offerText1 = string.Empty;
        private string offerText2 = string.Empty;
        private string offerText3 = string.Empty;
        private Int32 miles;
        private string displayMiles;

        public string OfferTitle
        {
            get
            {
                return this.offerTitle;
            }
            set
            {
                this.offerTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public decimal Price
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

        public string CurrencyCode
        {
            get
            {
                return this.currencyCode;
            }
            set
            {
                this.currencyCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string OfferText1
        {
            get
            {
                return this.offerText1;
            }
            set
            {
                this.offerText1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OfferText2
        {
            get
            {
                return this.offerText2;
            }
            set
            {
                this.offerText2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OfferText3
        {
            get
            {
                return this.offerText3;
            }
            set
            {
                this.offerText3 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public Int32 Miles
        {
            get
            {
                return this.miles;
            }
            set
            {
                this.miles = value;
            }
        }
        public string DisplayMiles
        {
            get
            {
                return this.displayMiles;
            }
            set
            {
                this.displayMiles = value;
            }
        }
    }
    [Serializable]
    public class MOBPASummary
    {
        private List<MOBTypeOption> existingCards;
        private decimal total;

        public List<MOBTypeOption> ExistingCards
        {
            get { return existingCards; }
            set { existingCards = value; }
        }

        public decimal Total
        {
            get { return total; }
            set { total = value; }
        }
    }
}
