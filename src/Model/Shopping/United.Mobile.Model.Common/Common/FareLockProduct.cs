using System;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    [XmlRoot("MOBSHOPFareLockProduct")]
    public class FareLockProduct
    {
        private string fareLockProductTitle = string.Empty;
        public string FareLockProductTitle
        {
            get { return fareLockProductTitle; }
            set { fareLockProductTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string fareLockProductAmountDisplayText = string.Empty;
        public string FareLockProductAmountDisplayText
        {
            get { return fareLockProductAmountDisplayText; }
            set { fareLockProductAmountDisplayText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private double fareLockProductAmount = 0.0;
        public double FareLockProductAmount
        {
            get { return fareLockProductAmount; }
            set { fareLockProductAmount = value; }
        }
        private string productCode = string.Empty;
        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }
        private string productId = string.Empty;
        public string ProductId
        {
            get { return productId; }
            set { productId = value; }
        }
        private string fareLockProductCode = string.Empty;

        public string FareLockProductCode
        {
            get { return fareLockProductCode; }
            set { fareLockProductCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

    }
}
