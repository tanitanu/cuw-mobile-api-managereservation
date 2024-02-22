using System;

namespace United.Mobile.Model.ManageRes
{
    [Serializable]
    public class MOBBasePrice
    {
        private string amount;
        private string code;
        private string description;
        private string currencyCode;
        private int decimalPlace;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public int DecimalPlace
        {
            get { return decimalPlace; }
            set { decimalPlace = value; }
        }

        public string Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
