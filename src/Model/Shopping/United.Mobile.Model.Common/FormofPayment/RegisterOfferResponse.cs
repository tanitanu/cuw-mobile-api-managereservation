using System;
using System.Collections.Generic;
using United.Mobile.Model.Common.Shopping;


namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class RegisterOfferResponse : MOBResponse
    {
        private string sessionId = string.Empty;
        private string flow = string.Empty;
        private bool isDefaultPaymentOption = false;
        private List<FormofPaymentOption> eligibleFormofPayments;
        private MOBShoppingCart shoppingCart = new MOBShoppingCart();
        private string pkDispenserPublicKey;
        private List<MOBFSRAlertMessage> alertMessages;


        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Flow
        {
            get
            {
                return this.flow;
            }
            set
            {
                this.flow = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public bool IsDefaultPaymentOption
        {
            get
            {
                return this.isDefaultPaymentOption;
            }
            set
            {
                this.isDefaultPaymentOption = value;
            }
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
        public string PkDispenserPublicKey
        {
            get { return pkDispenserPublicKey; }
            set { pkDispenserPublicKey = value; }
        }

        public List<MOBFSRAlertMessage> AlertMessages
        {
            get { return alertMessages; }
            set { alertMessages = value; }
        }
    }
}
