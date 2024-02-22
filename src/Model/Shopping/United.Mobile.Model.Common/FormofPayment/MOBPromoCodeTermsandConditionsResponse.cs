using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class PromoCodeTermsandConditionsResponse : MOBShoppingResponse
    {
        private MOBMobileCMSContentMessages termsandConditions;

        public MOBMobileCMSContentMessages TermsandConditions
        {
            get { return termsandConditions; }
            set { termsandConditions = value; }
        }

    }
}
