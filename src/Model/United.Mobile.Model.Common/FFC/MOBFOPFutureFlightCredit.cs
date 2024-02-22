using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBFOPFutureFlightCredit
    {
        private string pinCode;
        private string yearIssued;
        private string recipientsLastName;
        private string recipientsFirstName;
        private string travelerNameIndex;
        private double redeemAmount;
        private double initialValue;
        private double currentValue;
        private int index;

        private bool isCertificateApplied;

        private bool isRemove = false;
        private string expiryDate;
        private string recordLocator;

        private string creditAmount;
        private int paxId;
        private double newValueAfterRedeem;
        private string displayRedeemAmount;
        private string displayNewValueAfterRedeem;
        private string promoCode;
        private bool isEligibleToRedeem;
        private bool isNameMatch;
        private bool isNameMatchWaiverApplied;
        private bool isTravelDateBeginsBeforeCertExpiry;

        [JsonIgnore]
        public bool IsEligibleToRedeem { get { return isEligibleToRedeem; } set { isEligibleToRedeem = value; } }

        [JsonIgnore]
        public bool IsNameMatch { get { return isNameMatch; } set { isNameMatch = value; } }

        [JsonIgnore]
        public bool IsNameMatchWaiverApplied { get { return isNameMatchWaiverApplied; } set { isNameMatchWaiverApplied = value; } }

        [JsonIgnore]
        public bool IsTravelDateBeginsBeforeCertExpiry { get { return isTravelDateBeginsBeforeCertExpiry; } set { isTravelDateBeginsBeforeCertExpiry = value; } }

        public string PromoCode
        {
            get { return promoCode; }
            set { promoCode = value; }
        }

        public int PaxId
        {
            get { return paxId; }
            set { paxId = value; }
        }

        [JsonProperty(PropertyName = "creditAmmount")]
        [JsonPropertyName("creditAmmount")]
        public string CreditAmount
        {
            get { return creditAmount; }
            set { creditAmount = value; }
        }

        public string RecordLocator
        {
            get { return recordLocator; }
            set { recordLocator = value; }
        }

        public string ExpiryDate
        {
            get { return expiryDate; }
            set { expiryDate = value; }
        }

        public bool IsRemove
        {
            get { return isRemove; }
            set { isRemove = value; }
        }

        public bool IsCertificateApplied
        {
            get { return isCertificateApplied; }
            set { isCertificateApplied = value; }
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public double NewValueAfterRedeem
        {
            get { return newValueAfterRedeem; }
            set { newValueAfterRedeem = value; }

        }


        public double RedeemAmount
        {
            get { return redeemAmount; }
            set { redeemAmount = value; }
        }

        public string DisplayRedeemAmount
        {
            get { return displayRedeemAmount; }
            set { displayRedeemAmount = value; }
        }

        public string DisplayNewValueAfterRedeem
        {
            get { return displayNewValueAfterRedeem; }
            set { displayNewValueAfterRedeem = value; }
        }

        public double CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value; }
        }

        public double InitialValue
        {
            get { return initialValue; }
            set { initialValue = value; }
        }


        public string PinCode
        {
            get { return pinCode; }
            set { pinCode = value; }
        }

        public string YearIssued
        {
            get { return yearIssued; }
            set { yearIssued = value; }
        }

        public string RecipientsLastName
        {
            get { return recipientsLastName; }
            set { recipientsLastName = value; }
        }

        public string RecipientsFirstName
        {
            get { return recipientsFirstName; }
            set { recipientsFirstName = value; }
        }
        public string TravelerNameIndex
        {
            get { return travelerNameIndex; }
            set { travelerNameIndex = value; }
        }

    }
}
