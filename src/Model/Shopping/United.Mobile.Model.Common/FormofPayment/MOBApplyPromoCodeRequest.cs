using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Shopping;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    public class ApplyPromoCodeRequest : ShoppingRequest
    {
        private List<MOBPromoCode> promoCodes;
        public List<MOBPromoCode> PromoCodes
        {
            get { return promoCodes; }
            set { promoCodes = value; }
        }
        //private bool continueToResetMoneyMiles;
        //public bool ContinueToResetMoneyMiles
        //{
        //    get { return continueToResetMoneyMiles; }
        //    set { continueToResetMoneyMiles = value; }
        //}
    }
}
