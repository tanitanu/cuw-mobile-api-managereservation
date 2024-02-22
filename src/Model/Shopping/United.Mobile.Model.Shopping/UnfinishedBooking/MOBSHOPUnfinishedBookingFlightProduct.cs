using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping.UnfinishedBooking
{
    [Serializable]
    public class MOBSHOPUnfinishedBookingFlightProduct
    {
        private string bookingCode;
        private string productType;
        private int tripIndex;
        private List<MOBSHOPUnfinishedBookingProductPrice> prices;


        public int TripIndex
        {
            get { return tripIndex; }
            set { tripIndex = value; }
        }

        public string ProductType
        {
            get { return productType; }
            set { productType = value; }
        }

        public string BookingCode
        {
            get { return bookingCode; }
            set { bookingCode = value; }
        }
        public List<MOBSHOPUnfinishedBookingProductPrice> Prices
        {
            get { return prices; }
            set { prices = value; }
        }


    }
    [Serializable]
    public class MOBSHOPUnfinishedBookingProductPrice
    {
        private decimal amount;
        private decimal amountAllPax;
        private decimal amountBase;
        private string currency;
        private string currencyAllPax;
        private string offerID;
        private string pricingType;
        private bool selected;
        private MOBSHOPUnfinishedBookingProductPriceDetail merchPriceDetail;
        private List<MOBSHOPUnfinishedBookingProductSegmentMapping> segmentMappings;


        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public string PricingType
        {
            get { return pricingType; }
            set { pricingType = value; }
        }


        public string OfferID
        {
            get { return offerID; }
            set { offerID = value; }
        }

        public string CurrencyAllPax
        {
            get { return currencyAllPax; }
            set { currencyAllPax = value; }
        }

        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }

        public decimal AmountBase
        {
            get { return amountBase; }
            set { amountBase = value; }
        }

        public decimal AmountAllPax
        {
            get { return amountAllPax; }
            set { amountAllPax = value; }
        }

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public MOBSHOPUnfinishedBookingProductPriceDetail MerchPriceDetail
        {
            get { return merchPriceDetail; }
            set { merchPriceDetail = value; }
        }
        public List<MOBSHOPUnfinishedBookingProductSegmentMapping> SegmentMappings
        {
            get { return segmentMappings; }
            set { segmentMappings = value; }
        }


    }
    [Serializable]
    public class MOBSHOPUnfinishedBookingProductPriceDetail
    {
        private string eddCode;
        private string productCode;

        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }

        public string EddCode
        {
            get { return eddCode; }
            set { eddCode = value; }
        }

    }
    [Serializable]
    public class MOBSHOPUnfinishedBookingProductSegmentMapping
    {
        private string origin;
        private string destination;
        private string bBxHash;
        private string upgradeStatus;
        private string upgradeTo;
        private string upgradeType;
        private string flightNumber;
        private string cabinDescription;
        private string segmentRefID;

        public string SegmentRefID
        {
            get { return segmentRefID; }
            set { segmentRefID = value; }
        }

        public string CabinDescription
        {
            get { return cabinDescription; }
            set { cabinDescription = value; }
        }

        public string FlightNumber
        {
            get { return flightNumber; }
            set { flightNumber = value; }
        }

        public string UpgradeType
        {
            get { return upgradeType; }
            set { upgradeType = value; }
        }

        public string UpgradeTo
        {
            get { return upgradeTo; }
            set { upgradeTo = value; }
        }

        public string UpgradeStatus
        {
            get { return upgradeStatus; }
            set { upgradeStatus = value; }
        }

        public string BBxHash
        {
            get { return bBxHash; }
            set { bBxHash = value; }
        }

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }
    }
}
