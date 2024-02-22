using System;
using System.Collections.Generic;
using United.Mobile.Model.Common.Shopping;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.ReShop
{
    [Serializable]
    public class MOBSHOPReservationResponse : MOBResponse
    {

        private MOBShoppingCart shoppingCart = new MOBShoppingCart();
        private List<FormofPaymentOption> eligibleFormofPayments;
        private MOBSHOPReservation reservation;

        public MOBSHOPReservation Reservation
        {
            get
            {
                return this.reservation;
            }
            set
            {
                this.reservation = value;
            }
        }
        public MOBShoppingCart ShoppingCart
        {
            get { return shoppingCart; }
            set { shoppingCart = value; }
        }
        public List<FormofPaymentOption> EligibleFormofPayments
        {
            get { return eligibleFormofPayments; }
            set { eligibleFormofPayments = value; }
        }


    }
}
