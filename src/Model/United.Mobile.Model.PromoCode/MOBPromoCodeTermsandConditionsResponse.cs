using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;
using United.Mobile.Model.TravelCredit;
using MOBMobileCMSContentMessages = United.Mobile.Model.TravelCredit.MOBMobileCMSContentMessages;

namespace United.Mobile.Model.PromoCode
{
    [Serializable()]
    public class MOBPromoCodeTermsandConditionsResponse : MOBShoppingResponse
    {
        private MOBMobileCMSContentMessages termsandConditions;

        public MOBMobileCMSContentMessages TermsandConditions
        {
            get { return termsandConditions; }
            set { termsandConditions = value; }
        }
    }
}
