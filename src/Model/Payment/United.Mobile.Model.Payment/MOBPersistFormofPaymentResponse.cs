using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.FormofPayment;

namespace United.Mobile.Model.Payment
{
    [Serializable()]
    public class MOBPersistFormofPaymentResponse : MOBShoppingResponse
    {
        private MOBShoppingCart shoppingCart;
        private List<Common.Shopping.FormofPaymentOption> eligibleFormofPayments;
        private string pkDispenserPublicKey;
        private MOBSHOPReservation reservation;
        private List<CPProfile> profiles;

        public MOBShoppingCart ShoppingCart
        {
            get { return shoppingCart; }
            set { shoppingCart = value; }
        }
        public List<Common.Shopping.FormofPaymentOption> EligibleFormofPayments
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
