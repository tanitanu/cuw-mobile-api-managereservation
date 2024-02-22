using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Shopping;
using Newtonsoft.Json;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class FOPTravelerCertificateResponse : ShoppingResponse
    {
        [JsonIgnore]
        public string ObjectName { get; set; } = "United.Definition.FormofPayment.MOBFOPTravelerCertificateResponse";
        private List<MOBTypeOption> captions;
        private List<FormofPaymentOption> eligibleFormofPayments;
        private MOBShoppingCart shoppingCart = new MOBShoppingCart();
        private MOBSHOPReservation reservation;
        private List<CPProfile> profiles;
        private string pkDispenserPublicKey;
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

        public List<MOBTypeOption> Captions
        {
            get { return this.captions; }
            set { this.captions = value; }
        }

        public List<FormofPaymentOption> EligibleFormofPayments
        {
            get { return eligibleFormofPayments; }
            set { eligibleFormofPayments = value; }
        }
        public MOBShoppingCart ShoppingCart
        {
            get { return shoppingCart; }
            set { shoppingCart = value; }
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
