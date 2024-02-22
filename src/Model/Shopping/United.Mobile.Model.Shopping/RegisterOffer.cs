using System;
using System.Collections.Generic;
namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class RegisterOffer
    {
        private string productCode = string.Empty;
        private string productId = string.Empty;
        private List<string> productIds;
        private string subProductCode = string.Empty;

        public string ProductCode
        {
            get
            {
                return this.productCode;
            }
            set
            {
                this.productCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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

        public List<string> ProductIds { get; set; }

        public string SubProductCode
        {
            get
            {
                return this.subProductCode;
            }
            set
            {
                this.subProductCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public RegisterOffer()
        {
            ProductIds = new List<string>();
        }
    }

    [Serializable]
    public class BundleRegisterOffer
    {
        private string productCode = string.Empty;
        private string productId;
        private List<string> tripIds;
        List<string> selectedTripProductIDs { get; set; }

        public string ProductCode
        {
            get
            {
                return this.productCode;
            }
            set
            {
                this.productCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string ProductId { get; set; } = string.Empty;

        public List<string> TripIds { get; set; }

        public int ProductIndex { get; set; }

        public List<string> SelectedTripProductIDs { get; set; }

        public BundleRegisterOffer()
        {
            selectedTripProductIDs = new List<string>();
            TripIds = new List<string>();
            SelectedTripProductIDs = new List<string>();
        }
    }
}
