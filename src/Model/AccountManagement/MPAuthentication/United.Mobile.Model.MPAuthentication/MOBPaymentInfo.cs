using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBPaymentInfo
    {
        private string key = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private string cardType = string.Empty;
        private string cardTypeDescription = string.Empty;
        private string expireMonth = string.Empty;
        private string expireYear = string.Empty;
        private bool isPartnerCard;
        private string partnerCode = string.Empty;
        private string partnerCardType = string.Empty;
        private string issuer = string.Empty;

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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
                this.cardType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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

        public bool IsPartnerCard
        {
            get
            {
                return this.isPartnerCard;
            }
            set
            {
                this.isPartnerCard = value;
            }
        }

        public string PartnerCode
        {
            get
            {
                return this.partnerCode;
            }
            set
            {
                this.partnerCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string PartnerCardType
        {
            get
            {
                return this.partnerCardType;
            }
            set
            {
                this.partnerCardType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Issuer
        {
            get
            {
                return this.issuer;
            }
            set
            {
                this.issuer = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
