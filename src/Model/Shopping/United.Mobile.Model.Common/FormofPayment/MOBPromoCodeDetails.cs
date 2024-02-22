using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class MOBPromoCodeDetails
    {

        private List<MOBPromoCode> promoCodes;

        public List<MOBPromoCode> PromoCodes
        {
            get { return promoCodes; }
            set { promoCodes = value; }
        }
        private bool isDisablePromoOption;

        public bool IsDisablePromoOption
        {
            get { return isDisablePromoOption; }
            set { isDisablePromoOption = value; }
        }
        private bool isHidePromoOption;

        public bool IsHidePromoOption
        {
            get { return isHidePromoOption; }
            set { isHidePromoOption = value; }
        }



    }
}
