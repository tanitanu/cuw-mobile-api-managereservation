using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBCreditCard
    {
        private string key = string.Empty;
        private string cardType = string.Empty;
        private string cardTypeDescription = string.Empty;
        private string description = string.Empty;
        private string expireMonth = string.Empty;
        private string expireYear = string.Empty;
        private bool isPrimary;
        private string unencryptedCardNumber = string.Empty;
        private string encryptedCardNumber = string.Empty;
        private string displayCardNumber = string.Empty;
        private string cIDCVV2 = string.Empty;
        private string cCName = string.Empty;
        private string addressKey = string.Empty;
        private string phoneKey = string.Empty;
        private string message = string.Empty;
        private string accountNumberToken = string.Empty;
        private string persistentToken = string.Empty;
        private string securityCodeToken = string.Empty;
        private string barCode = string.Empty;
        private bool isCorporate;
        private bool isMandatory;
        private string billedSeperateText;
        private bool isValidForTPIPurchase;
        private bool isOAEPPaddingCatalogEnabled;

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CardType
        {
            get
            {
                return this.cardType;
            }
            set
            {
                this.cardType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string CardTypeDescription
        {
            get
            {
                return this.cardTypeDescription;
            }
            set
            {
                this.cardTypeDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ExpireMonth
        {
            get
            {
                return this.expireMonth;
            }
            set
            {
                this.expireMonth = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ExpireYear
        {
            get
            {
                return this.expireYear;
            }
            set
            {
                this.expireYear = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool IsPrimary
        {
            get
            {
                return this.isPrimary;
            }
            set
            {
                this.isPrimary = value;
            }
        }
        public string UnencryptedCardNumber
        {
            get { return this.unencryptedCardNumber; }
            set { this.unencryptedCardNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string EncryptedCardNumber
        {
            get { return this.encryptedCardNumber; }
            set { this.encryptedCardNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string DisplayCardNumber
        {
            get { return this.displayCardNumber; }
            set { this.displayCardNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string CIDCVV2
        {
            get { return this.cIDCVV2; }
            set { this.cIDCVV2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string CCName
        {
            get { return this.cCName; }
            set { this.cCName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string AddressKey
        {
            get
            {
                return this.addressKey;
            }
            set
            {
                this.addressKey = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PhoneKey
        {
            get
            {
                return this.phoneKey;
            }
            set
            {
                this.phoneKey = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string AccountNumberToken
        {
            get
            {
                return this.accountNumberToken;
            }
            set
            {
                this.accountNumberToken = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string PersistentToken
        {
            get
            {
                return this.persistentToken;
            }
            set
            {
                this.persistentToken = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string SecurityCodeToken
        {
            get
            {
                return this.securityCodeToken;
            }
            set
            {
                this.securityCodeToken = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string BarCode
        {
            get
            {
                return this.barCode;
            }
            set
            {
                this.barCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public bool IsCorporate
        {
            get
            {
                return isCorporate;
            }
            set
            {
                isCorporate = value;
            }
        }
        public bool IsMandatory
        {
            get
            {
                return isMandatory;
            }
            set
            {
                isMandatory = value;
            }
        }
        public string BilledSeperateText
        {
            get
            {
                return this.billedSeperateText;
            }
            set
            {
                this.billedSeperateText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public bool IsValidForTPIPurchase
        {
            get
            {
                return isValidForTPIPurchase;
            }
            set
            {
                isValidForTPIPurchase = value;
            }
        }
        public bool IsOAEPPaddingCatalogEnabled
        {
            get
            {
                return isOAEPPaddingCatalogEnabled;
            }
            set
            {
                isOAEPPaddingCatalogEnabled = value;
            }
        }
    }

    [Serializable]
    public enum MOBFormofPayment
    {
        [EnumMember(Value = "CreditCard")]
        CreditCard,
        [EnumMember(Value = "PayPal")]
        PayPal,
        [EnumMember(Value = "PayPalCredit")]
        PayPalCredit,
        [EnumMember(Value = "ApplePay")]
        ApplePay,
        [EnumMember(Value = "Masterpass")]
        Masterpass,
        [EnumMember(Value = "VisaCheckout")]
        VisaCheckout,
        [EnumMember(Value = "MilesFOP")]
        MilesFormOfPayment,
        [EnumMember(Value = "ETC")]
        ETC,
        [EnumMember(Value = "Uplift")]
        Uplift,
        [EnumMember(Value = "FFC")]
        FFC
    }

    
    [Serializable]
    public class MOBVormetricKeys
    {
        private string accountNumberToken = string.Empty;
        private string persistentToken = string.Empty;
        private string securityCodeToken = string.Empty;
        private string cardType = string.Empty;

        public string AccountNumberToken
        {
            get
            {
                return this.accountNumberToken;
            }
            set
            {
                this.accountNumberToken = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PersistentToken
        {
            get
            {
                return this.persistentToken;
            }
            set
            {
                this.persistentToken = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string SecurityCodeToken
        {
            get
            {
                return this.securityCodeToken;
            }
            set
            {
                this.securityCodeToken = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CardType
        {
            get
            {
                return this.cardType;
            }
            set
            {
                this.cardType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
