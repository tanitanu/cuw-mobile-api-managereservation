using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Shopping;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class ApplyPromoCodeResponse : MOBShoppingResponse
    {
        public string ObjectName { get; set; } = "United.Definition.FormofPayment.MOBApplyPromoCodeResponse";
        private MOBShoppingCart shoppingCart = new MOBShoppingCart();
        private MOBSHOPReservation reservation;
        private List<FormofPaymentOption> eligibleFormofPayments;
        private string maxCountMessage;
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
