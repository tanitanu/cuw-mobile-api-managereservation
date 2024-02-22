using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class CustomerProfileResponse: CPProfileResponse
    {        
        private string transactionPath = string.Empty;
        private string redirectViewName = string.Empty;
        private string pKDispenserPublicKey = string.Empty;
        private MOBCreditCard selectedCreditCard;
        private MOBAddress selectedAddress;

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

        public string PKDispenserPublicKey
        {
            get { return pKDispenserPublicKey; }
            set { pKDispenserPublicKey = value; }
        }

        public string RedirectViewName
        {
            get { return redirectViewName; }
            set { redirectViewName = value; }
        }
        public string TransactionPath
        {
            get { return transactionPath; }
            set { transactionPath = value; }
        }        
    }
}
