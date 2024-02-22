using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBPromoCode
    {
        private string promoCode;
        public string PromoCode
        {
            get { return promoCode; }
            set { promoCode = value; }
        }
        private bool isRemove;
        public bool IsRemove
        {
            get { return isRemove; }
            set { isRemove = value; }
        }
        private string alertMessage;

        public string AlertMessage
        {
            get { return alertMessage; }
            set { alertMessage = value; }
        }
        private bool isSuccess;

        public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }

        private MOBMobileCMSContentMessages termsandConditions;

        public MOBMobileCMSContentMessages TermsandConditions
        {
            get { return termsandConditions; }
            set { termsandConditions = value; }
        }
        private double promoValue;

        public double PromoValue
        {
            get { return promoValue; }
            set { promoValue = value; }
        }
        private string formattedPromoDisplayValue;

        public string FormattedPromoDisplayValue
        {
            get { return formattedPromoDisplayValue; }
            set { formattedPromoDisplayValue = value; }
        }
        private string priceTypeDescription;

        public string PriceTypeDescription
        {
            get { return priceTypeDescription; }
            set { priceTypeDescription = value; }
        }
    }
}
