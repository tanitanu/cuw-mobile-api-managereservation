using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Shopping;
using Newtonsoft.Json;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class FutureFlightCreditResponse : MOBShoppingResponse
    {
        [JsonIgnore]
        public string ObjectName { get; set; } = "United.Definition.FormofPayment.MOBFutureFlightCreditResponse";

        private List<MOBTypeOption> captions;
        private List<FormofPaymentOption> eligibleFormofPayments;
        private MOBShoppingCart shoppingCart = new MOBShoppingCart();
        private MOBSHOPReservation reservation;
        private List<CPProfile> profiles;
        private string pkDispenserPublicKey;
        //private string webShareToken = string.Empty;
        //private string webSessionShareUrl = string.Empty;

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

        //public string WebShareToken { get { return this.webShareToken; } set { this.webShareToken = value; } }
        //public string WebSessionShareUrl { get { return this.webSessionShareUrl; } set { this.webSessionShareUrl = value; } }

    }



}
