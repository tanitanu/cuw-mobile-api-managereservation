using Newtonsoft.Json;
using System;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBCustomerProfileResponse : MOBCPProfileResponse
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

        [JsonProperty(PropertyName = "pKDispenserPublicKey")]

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
