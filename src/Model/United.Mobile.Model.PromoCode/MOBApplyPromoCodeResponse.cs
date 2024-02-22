using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Payment;
//using United.Mobile.Model.Common.Shopping;
using United.Mobile.Model.TravelCredit;
using FormofPaymentOption = United.Mobile.Model.TravelCredit.FormofPaymentOption;
using MOBSection = United.Mobile.Model.TravelCredit.MOBSection;
//using MOBSHOPReservation = United.Mobile.Model.Shopping.MOBSHOPReservation;

namespace United.Mobile.Model.PromoCode
{
    [Serializable()]
    public class MOBApplyPromoCodeResponse : MOBShoppingResponse
    {
        private MOBShoppingCart shoppingCart = new MOBShoppingCart();
        private MOBSHOPReservation reservation;
        private List<FormofPaymentOption> eligibleFormofPayments;
        private string maxCountMessage;
        private MOBSection promoCodeRemovalAlertMessage;

        public MOBSection PromoCodeRemovalAlertMessage
        {
            get { return promoCodeRemovalAlertMessage; }
            set { promoCodeRemovalAlertMessage = value; }
        }

        public string MaxCountMessage
        {
            get { return maxCountMessage; }
            set { maxCountMessage = value; }
        }

        public MOBShoppingCart ShoppingCart
        {
            get { return shoppingCart; }
            set { shoppingCart = value; }
        }
        public MOBSHOPReservation Reservation
        {
            get { return reservation; }
            set { this.reservation = value; }
        }
        public List<FormofPaymentOption> EligibleFormofPayments
        {
            get { return eligibleFormofPayments; }
            set { eligibleFormofPayments = value; }
        }
    }
}
