using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping.UpgradeCabin
{
    [Serializable]
    public class UpgradeCabinPriceDetails
    {
        private List<MOBSHOPPrice> prices;
        private string paymentDeductionsMsg;
        public List<MOBSHOPPrice> Prices { get { return this.prices; } set { this.prices = value; } }
        public string PaymentDeductionsMsg { get { return this.paymentDeductionsMsg; } set { this.paymentDeductionsMsg = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
    }
}
