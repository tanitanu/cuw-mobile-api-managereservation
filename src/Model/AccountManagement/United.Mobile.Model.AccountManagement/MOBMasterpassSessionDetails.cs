using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.MPSignIn
{
    [Serializable]
    public class MOBMasterpassSessionDetails
    {
        private string contactEmailAddress;
        private string contactPhoneNumber;
        private string givenName;
        private string surname;
        private string status;
        private string accountNumber;
        private string accountNumberEncrypted;
        private string accountNumberHMAC;
        private string accountNumberLastFourDigits;
        private string accountNumberMasked;
        private string accountNumberToken;
        private string persistentToken;
        private double amount;
        private MOBAddress billingAddress;
        private string operationID;
        private int walletCategory;
        private string code;
        private int creditCardTypeCode;
        private int description;
        private string expirationDate;
        private string name;
        private MOBMasterpassType masterpassType;

        public MOBMasterpassType MasterpassType
        {
            get { return masterpassType; }
            set { masterpassType = value; }
        }

        public string ContactEmailAddress
        {
            get { return contactEmailAddress; }
            set { contactEmailAddress = value; }
        }

        public string ContactPhoneNumber
        {
            get { return contactPhoneNumber; }
            set { contactPhoneNumber = value; }
        }

        public string SurName
        {
            get { return surname; }
            set { surname = value; }
        }

        public string GivenName
        {
            get { return givenName; }
            set { givenName = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }


        public string AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = value; }
        }

        public string AccountNumberEncrypted
        {
            get { return accountNumberEncrypted; }
            set { accountNumberEncrypted = value; }
        }

        public string AccountNumberHMAC
        {
            get { return accountNumberHMAC; }
            set { accountNumberHMAC = value; }
        }

        public string AccountNumberLastFourDigits
        {
            get { return accountNumberLastFourDigits; }
            set { accountNumberLastFourDigits = value; }
        }

        public string AccountNumberMasked
        {
            get { return accountNumberMasked; }
            set { accountNumberMasked = value; }
        }

        public string AccountNumberToken
        {
            get { return accountNumberToken; }
            set { accountNumberToken = value; }
        }
        public string PersistentToken
        {
            get { return persistentToken; }
            set { persistentToken = value; }
        }
        public double Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public MOBAddress BillingAddress
        {
            get { return billingAddress; }
            set { billingAddress = value; }
        }

        public string OperationID
        {
            get { return operationID; }
            set { operationID = value; }
        }

        public int WalletCategory
        {
            get { return walletCategory; }
            set { walletCategory = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public int CreditCardTypeCode
        {
            get { return creditCardTypeCode; }
            set { creditCardTypeCode = value; }
        }

        public int Description
        {
            get { return description; }
            set { description = value; }
        }

        public string ExpirationDate
        {
            get { return expirationDate; }
            set { expirationDate = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    public class MOBMasterpassType
    {
        private string defaultIndicator;
        private string description;
        private string key;
        private string val;

        public string DefaultIndicator
        {
            get { return defaultIndicator; }
            set { defaultIndicator = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        public string Val
        {
            get { return val; }
            set { val = value; }
        }
    }
}
