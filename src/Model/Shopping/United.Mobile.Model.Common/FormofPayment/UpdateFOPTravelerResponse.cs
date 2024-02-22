using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Shopping;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class UpdateFOPTravelerResponse : CPProfileResponse
    {
        private string flow = string.Empty;
        private List<FormofPaymentOption> eligibleFormofPayments;
        private MOBCreditCard selectedCreditCard;
        private MOBAddress selectedAddress;
        private string pkDispenserPublicKey;

        
        public string Flow
        {
            get { return flow; }
            set { flow = value; }
        }
        
        public List<FormofPaymentOption> EligibleFormofPayments
        {
            get { return eligibleFormofPayments; }
            set { eligibleFormofPayments = value; }
        }
        
        public MOBCreditCard SelectedCreditCard
        {
            set { selectedCreditCard = value; }
            get { return selectedCreditCard; }
        }
        public MOBAddress SelectedAddress
        {
            set { selectedAddress = value; }
            get { return selectedAddress; }
        }
        public string PkDispenserPublicKey
        {
            get { return pkDispenserPublicKey; }
            set { pkDispenserPublicKey = value; }
        }
    }
}
