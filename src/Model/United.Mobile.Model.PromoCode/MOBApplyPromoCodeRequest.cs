using System;
using System.Collections.Generic;
using United.Mobile.Model.TravelCredit;

namespace United.Mobile.Model.PromoCode
{
    [Serializable()]
    public class MOBApplyPromoCodeRequest : MOBShoppingRequest
    {
        private List<MOBPromoCode> promoCodes;
        public List<MOBPromoCode> PromoCodes
        {
            get { return promoCodes; }
            set { promoCodes = value; }
        }

        private bool continueToResetMoneyMiles;
        public bool ContinueToResetMoneyMiles
        {
            get { return continueToResetMoneyMiles; }
            set { continueToResetMoneyMiles = value; }
        }
    }
}
