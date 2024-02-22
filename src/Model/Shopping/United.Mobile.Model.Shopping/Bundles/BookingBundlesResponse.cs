using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Bundles
{
    [Serializable]
    [XmlRoot("MOBBookingBundlesResponse")]
    public class BookingBundlesResponse : MOBResponse
    {
        [XmlIgnore]
        public string ObjectName { get; set; } = "United.Definition.Shopping.Bundles.MOBBookingBundlesResponse";
        private readonly IConfiguration _configuration;
        public BookingBundlesResponse (IConfiguration configuration)
        {
            _configuration = configuration;
            BookingBundlesTitle = configuration.GetValue<string>("BookingBundlesNoBundlesScreenTitle") ?? string.Empty;           
        }

        public BookingBundlesResponse()
        {

        }
        //public string ObjectName = "United.Definition.Shopping.Bundles.MOBBookingBundlesResponse";

        private int totalAmount;
        private List<BundleProduct> products;
        private string bookingBundlesTitle = string.Empty;
        private MOBMobileCMSContentMessages termsAndCondition;
        private string flow;
        private string sessionId;
        private string cartId;
        private TPIInfoInBookingPath tripInsuranceInfoBookingPath;
        private string clearOption = string.Empty;
        public List<BundleProduct> Products
        {
            get { return products; }
            set
            {
                products = value;

                if (_configuration != null && (_configuration.GetValue<string>("BookingBundlesScreenTitleEnabled") ?? string.Empty) == "true")
                {
                    if (products != null && products.Count > 0)
                    {
                        BookingBundlesTitle = _configuration.GetValue<string>("BookingBundlesScreenTitle") ?? string.Empty;
                    }
                    else
                    {
                        BookingBundlesTitle = _configuration.GetValue<string>("BookingBundlesNoBundlesScreenTitle") ?? string.Empty;
                    }
                }
            }
        }
        public string BookingBundlesTitle
        {
            get
            {
                if (_configuration != null && (_configuration.GetValue<string>("BookingBundlesScreenTitleEnabled") ?? string.Empty) == "true")
                {
                    if (products != null && products.Count > 0)
                    {
                        bookingBundlesTitle = _configuration.GetValue<string>("BookingBundlesScreenTitle") ?? string.Empty;
                    }
                    else
                    {
                        bookingBundlesTitle = _configuration.GetValue<string>("BookingBundlesNoBundlesScreenTitle") ?? string.Empty;
                    }
                }
                return bookingBundlesTitle;
            }
            set { bookingBundlesTitle = value; }
        }
        public MOBMobileCMSContentMessages TermsAndCondition
        {
            get { return termsAndCondition; }
            set { termsAndCondition = value; }
        }
        public int TotalAmount
        {
            get { return totalAmount; }
            set { totalAmount = value; }
        }
        public string Flow
        {
            get { return flow; }
            set { flow = value; }
        }
        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }
        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }

        public TPIInfoInBookingPath TripInsuranceInfoBookingPath
        {
            get { return this.tripInsuranceInfoBookingPath; }
            set { this.tripInsuranceInfoBookingPath = value; }
        }
        public string ClearOption
        {
            get { return this.clearOption; }
            set { clearOption = value; }
        }

    }

    #region NewBundles
    [Serializable]
    public class BundleProduct
    {
        private string productID;
        private string productCode;
        private List<string> productIDs;
        private BundleTile tile;
        private BundleDetail detail;
        private int amount;
        private int productIndex;
        private List<string> bundleProductCodes;

        public List<string> BundleProductCodes
        {
            get { return bundleProductCodes; }
            set { bundleProductCodes = value; }
        }

        public string ProductID
        {
            get { return productID; }
            set { productID = value; }
        }
        public List<string> ProductIDs
        {
            get { return productIDs; }
            set { productIDs = value; }
        }
        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }

        public BundleTile Tile
        {
            get { return tile; }
            set { tile = value; }
        }

        public BundleDetail Detail
        {
            get { return detail; }
            set { detail = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public int ProductIndex
        {
            get { return productIndex; }
            set { productIndex = value; }
        }
    }

    [Serializable]
    public class BundleTile
    {
        public string OfferTitle { get; set; } = string.Empty;
        public List<string> OfferDescription { get; set; }
        public string PriceText { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
        public int TileIndex { get; set; }
        public string OfferPrice { get; set; } = string.Empty;
        private string bundleBadgeText;
        public BundleTile()
        {
            OfferDescription = new List<string>();
        }
        public string BundleBadgeText
        {
            get { return bundleBadgeText; }
            set { bundleBadgeText = value; }
        }
    }

    [Serializable]
    public class BundleDetail
    {

        public string OfferTitle { get; set; } = string.Empty;
        public string OfferWarningMessage { get; set; }
        public List<BundleOfferDetail> OfferDetails { get; set; }
        public List<BundleOfferTrip> OfferTrips { get; set; }
        public BundleDetail()
        {
            OfferDetails = new List<BundleOfferDetail>();
            OfferTrips = new List<BundleOfferTrip>();
        }
    }

    [Serializable]
    public class BundleOfferDetail
    {
        public string OfferDetailHeader { get; set; } = string.Empty;
        public string OfferDetailDescription { get; set; } = string.Empty;
        public string OfferDetailWarningMessage { get; set; } = string.Empty;

    }

    [Serializable]
    public class BundleOfferTrip
    {

        public string OriginDestination { get; set; } = string.Empty;
        public string TripId { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
        public int Price { get; set; }
        public string TripProductID { get; set; } = string.Empty;
        public List<string> TripProductIDs { get; set; }
        public BundleOfferTrip()
        {
            TripProductIDs = new List<string>();
        }
    }

    #endregion



}
