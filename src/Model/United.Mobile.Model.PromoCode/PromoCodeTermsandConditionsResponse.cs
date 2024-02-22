using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.PromoCode
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
