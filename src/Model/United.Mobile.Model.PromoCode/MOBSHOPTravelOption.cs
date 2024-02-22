using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.PromoCode
{

    [Serializable]
    public class MOBSHOPTravelOption
    {
        private double amount;
        private string displayAmount = string.Empty;
        private string displayButtonAmount = string.Empty;
        private string currencyCode = string.Empty;
        private bool deleted;
        private string description = string.Empty;
        private string key = string.Empty;
        private string productId = string.Empty;
        private List<MOBSHOPTravelOptionSubItem> subItems;
        private string type = string.Empty;
        private List<string> tripIds;

        private string bundleOfferTitle = string.Empty;
        private List<string> bundleOfferDescription;
        private List<MobShopBundleEplus> bundleCode;
        private string code;

        private string bundleOfferSubtitle;

        private List<MOBAncillaryDescriptionItem> ancillaryDescriptionItems;
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        private MOBPromoCode promoDetails;

        public MOBPromoCode PromoDetails
        {
            get { return promoDetails; }
            set { promoDetails = value; }
        }

        public double Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                this.amount = value;
            }
        }

        public string DisplayAmount
        {
            get
            {
                return this.displayAmount;
            }
            set
            {
                this.displayAmount = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string DisplayButtonAmount
        {
            get
            {
                return this.displayButtonAmount;
            }
            set
            {
                this.displayButtonAmount = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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

        public bool Deleted
        {
            get
            {
                return this.deleted;
            }
            set
            {
                this.deleted = value;
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

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ProductId
        {
            get
            {
                return this.productId;
            }
            set
            {
                this.productId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBSHOPTravelOptionSubItem> SubItems
        {
            get
            {
                return this.subItems;
            }
            set
            {
                this.subItems = value;
            }
        }

        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }


        public List<string> TripIds
        {
            get
            {
                return this.tripIds;
            }
            set
            {
                this.tripIds = value;
            }
        }


        public string BundleOfferTitle
        {
            get
            {
                return this.bundleOfferTitle;
            }
            set
            {
                this.bundleOfferTitle = value;
            }
        }

        public List<string> BundleOfferDescription
        {
            get
            {
                return this.bundleOfferDescription;
            }
            set
            {
                this.bundleOfferDescription = value;
            }
        }


        public List<MobShopBundleEplus> BundleCode
        {
            get
            {
                return this.bundleCode;
            }
            set
            {
                this.bundleCode = value;
            }
        }
        public string BundleOfferSubtitle
        {
            get { return bundleOfferSubtitle; }
            set { bundleOfferSubtitle = value; }
        }

        public List<MOBAncillaryDescriptionItem> AncillaryDescriptionItems
        {
            get { return this.ancillaryDescriptionItems; }
            set { this.ancillaryDescriptionItems = value; }
        }

    }


    [Serializable]
    public class MobShopBundleEplus
    {
        private string proudtkey = string.Empty;
        private string segmentName = string.Empty;
        private int associatedTripIndex;

        public string ProductKey
        {
            get
            {
                return this.proudtkey;
            }
            set
            {
                this.proudtkey = value;
            }
        }

        public string SegmentName
        {
            get
            {
                return this.segmentName;
            }
            set
            {
                this.segmentName = value;
            }
        }

        public int AssociatedTripIndex
        {
            get { return associatedTripIndex; }
            set { associatedTripIndex = value; }
        }
    }
    [Serializable]
    public class MOBAncillaryDescriptionItem
    {
        private string title;
        private string subTitle;
        private string displayValue;

        public string DisplayValue
        {
            get { return displayValue; }
            set { displayValue = value; }
        }


        public string SubTitle
        {
            get { return subTitle; }
            set { subTitle = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

    }
}
