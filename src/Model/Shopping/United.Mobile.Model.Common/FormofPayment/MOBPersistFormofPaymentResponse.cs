using System;
using System.Collections.Generic;
using United.Mobile.Model.Common.Shopping;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class PersistFormofPaymentResponse : ShoppingResponse
    {
        private MOBShoppingCart shoppingCart;
        private List<FormofPaymentOption> eligibleFormofPayments;
        private string pkDispenserPublicKey;
        private MOBSHOPReservation reservation;
        private List<CPProfile> profiles;

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
        public string PkDispenserPublicKey
        {
            get { return pkDispenserPublicKey; }
            set { pkDispenserPublicKey = value; }
        }
        public MOBSHOPReservation Reservation
        {
            get { return reservation; }
            set { this.reservation = value; }
        }
        public List<CPProfile> Profiles
        {
            get
            {
                return profiles;
            }
            set
            {
                this.profiles = value;
            }
        }
    }
}
