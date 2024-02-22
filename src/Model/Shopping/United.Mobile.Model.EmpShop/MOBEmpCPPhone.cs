using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBEmpCPPhone
    {
        private string areaNumber = string.Empty;
        private string attention = string.Empty;
        private string channelCode = string.Empty;
        private string channelCodeDescription = string.Empty;
        private string channelTypeCode = string.Empty;
        private string channelTypeDescription = string.Empty;
        private int channelTypeSeqNumber;
        private string countryCode = string.Empty;
        private string countryNumber = string.Empty;
        private string countryPhoneNumber = string.Empty;
        private int customerId;
        private string description = string.Empty;
        private string discontinuedDate = string.Empty;
        private string effectiveDate = string.Empty;
        private string extensionNumber = string.Empty;
        private bool isPrimary;
        private bool isPrivate;
        private bool isProfileOwner;
        private string key = string.Empty;
        private string languageCode = string.Empty;
        private string mileagePlusId = string.Empty;
        private string pagerPinNumber = string.Empty;
        private string phoneNumber = string.Empty;
        private string sharesCountryCode = string.Empty;
        private string wrongPhoneDate = string.Empty;
        private string deviceTypeCode = string.Empty;
        private string deviceTypeDescription = string.Empty;
        private bool isDayOfTravel;
        private bool isSMSEnabled;

        public string AreaNumber
        {
            get
            {
                return this.areaNumber;
            }
            set
            {
                this.areaNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Attention
        {
            get
            {
                return this.attention;
            }
            set
            {
                this.attention = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ChannelCode
        {
            get
            {
                return this.channelCode;
            }
            set
            {
                this.channelCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ChannelCodeDescription
        {
            get
            {
                return this.channelCodeDescription;
            }
            set
            {
                this.channelCodeDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ChannelTypeCode
        {
            get
            {
                return this.channelTypeCode;
            }
            set
            {
                this.channelTypeCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ChannelTypeDescription
        {
            get
            {
                return this.channelTypeDescription;
            }
            set
            {
                this.channelTypeDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int ChannelTypeSeqNumber
        {
            get
            {
                return this.channelTypeSeqNumber;
            }
            set
            {
                this.channelTypeSeqNumber = value;
            }
        }

        public string CountryCode
        {
            get
            {
                return this.countryCode;
            }
            set
            {
                this.countryCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CountryNumber
        {
            get
            {
                return this.countryNumber;
            }
            set
            {
                this.countryNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CountryPhoneNumber
        {
            get
            {
                return this.countryPhoneNumber;
            }
            set
            {
                this.countryPhoneNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int CustomerId
        {
            get
            {
                return this.customerId;
            }
            set
            {
                this.customerId = value;
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

        public string DiscontinuedDate
        {
            get
            {
                return this.discontinuedDate;
            }
            set
            {
                this.discontinuedDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EffectiveDate
        {
            get
            {
                return this.effectiveDate;
            }
            set
            {
                this.effectiveDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ExtensionNumber
        {
            get
            {
                return this.extensionNumber;
            }
            set
            {
                this.extensionNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public bool IsPrivate
        {
            get
            {
                return this.isPrivate;
            }
            set
            {
                this.isPrivate = value;
            }
        }

        public bool IsProfileOwner
        {
            get
            {
                return this.isProfileOwner;
            }
            set
            {
                this.isProfileOwner = value;
            }
        }

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

        public string LanguageCode
        {
            get
            {
                return this.languageCode;
            }
            set
            {
                this.languageCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MileagePlusId
        {
            get
            {
                return this.mileagePlusId;
            }
            set
            {
                this.mileagePlusId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PagerPinNumber
        {
            get
            {
                return this.pagerPinNumber;
            }
            set
            {
                this.pagerPinNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PhoneNumber
        {
            get
            {
                return this.phoneNumber;
            }
            set
            {
                this.phoneNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string SharesCountryCode
        {
            get
            {
                return this.sharesCountryCode;
            }
            set
            {
                this.sharesCountryCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string WrongPhoneDate
        {
            get
            {
                return this.wrongPhoneDate;
            }
            set
            {
                this.wrongPhoneDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DeviceTypeCode
        {
            get
            {
                return this.deviceTypeCode;
            }
            set
            {
                this.deviceTypeCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DeviceTypeDescription
        {
            get
            {
                return this.deviceTypeDescription;
            }
            set
            {
                this.deviceTypeDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool IsDayOfTravel
        {
            get
            {
                return this.isDayOfTravel;
            }
            set
            {
                this.isDayOfTravel = value;
            }
        }

        public bool IsSMSEnabled
        {
            get
            {
                return this.isSMSEnabled;
            }
            set
            {
                this.isSMSEnabled = value;
            }
        }
    }
}
