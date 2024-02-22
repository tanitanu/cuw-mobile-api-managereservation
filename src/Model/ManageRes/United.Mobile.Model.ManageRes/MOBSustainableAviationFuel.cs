using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.ManageRes
{
    public class MOBSustainableAviationFuel
    {
        private MOBSection description;

        public MOBSection Description
        {
            get { return description; }
            set { description = value; }
        }
        private string code;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        private List<MOBPriceItem> priceItems;

        public List<MOBPriceItem> PriceItems
        {
            get { return priceItems; }
            set { priceItems = value; }
        }

    }
    public class MOBPriceItem
    {
        private string productId;

        public string ProductId
        {
            get { return productId; }
            set { productId = value; }
        }
        private string subProductCode;

        public string SubProductCode
        {
            get { return subProductCode; }
            set { subProductCode = value; }
        }
        private string price;

        public string Price
        {
            get { return price; }
            set { price = value; }
        }
        private bool isDefault;

        public bool IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }
    }
}
