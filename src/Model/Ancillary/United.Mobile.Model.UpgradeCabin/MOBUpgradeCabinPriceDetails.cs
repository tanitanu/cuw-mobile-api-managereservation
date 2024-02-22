using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.SeatMap;

namespace United.Mobile.Model.UpgradeCabin
{
    [Serializable]
    public class MOBUpgradeCabinPriceDetails
    {
        private List<MOBSHOPPrice> prices;
        private string paymentDeductionsMsg;
        public List<MOBSHOPPrice> Prices { get { return this.prices; } set { this.prices = value; } }
        public string PaymentDeductionsMsg { get { return this.paymentDeductionsMsg; } set { this.paymentDeductionsMsg = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
    }
}
