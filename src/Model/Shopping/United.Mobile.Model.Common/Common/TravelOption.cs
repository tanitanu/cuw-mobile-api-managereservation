using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping.FormofPayment;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class TravelOption
    {
        public string Code { get; set; } = string.Empty;

        public MOBPromoCode PromoDetails { get; set; }
        public double Amount { get; set; }

        public string DisplayAmount { get; set; } = string.Empty;

        public string DisplayButtonAmount { get; set; } = string.Empty;

        public string CurrencyCode { get; set; } = string.Empty;

        public bool Deleted { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Key { get; set; } = string.Empty;

        public string ProductId { get; set; } = string.Empty;
        public List<TravelOptionSubItem> SubItems { get; set; }

        public string Type { get; set; } = string.Empty;

        public List<string> TripIds { get; set; }

        public List<string> BundleOfferDescription { get; set; }

        public List<ShopBundleEplus> BundleCode { get; set; }

        public string BundleOfferTitle { get; set; }

        public string BundleOfferSubtitle { get; set; }

        public List<AncillaryDescriptionItem> AncillaryDescriptionItems { get; set; }
        public TravelOption()
        {
            SubItems = new List<TravelOptionSubItem>();
            TripIds = new List<string>();
            BundleOfferDescription = new List<string>();
            BundleCode = new List<ShopBundleEplus>();
        }

    }

    [Serializable]
    public class ShopBundleEplus
    {
        
        public string ProductKey { get; set; } = string.Empty;
        
        public string SegmentName { get; set; } = string.Empty;

        public int AssociatedTripIndex { get; set; }
        
    }

    [Serializable]
    public class AncillaryDescriptionItem
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
