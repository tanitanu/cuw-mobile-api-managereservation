using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    public class MOBPromoAlertMessage
    {
        private string promoCode;
        public string PromoCode
        {
            get { return promoCode; }
            set { promoCode = value; }
        }

        private bool isSignInRequired;
        public bool IsSignInRequired
        {
            get { return isSignInRequired; }
            set { isSignInRequired = value; }
        }

        private MOBSection alertMessages;

        public MOBSection AlertMessages
        {
            get
            {
                return this.alertMessages;
            }
            set
            {
                this.alertMessages = value;
            }
        }

        private string termsandConditions;
        public string TermsandConditions
        {
            get { return termsandConditions; }
            set { termsandConditions = value; }
        }

        private string couponInlineAlertMessage;
        public string CouponInlineAlertMessage
        {
            get { return couponInlineAlertMessage; }
            set { couponInlineAlertMessage = value; }
        }
    }
}
