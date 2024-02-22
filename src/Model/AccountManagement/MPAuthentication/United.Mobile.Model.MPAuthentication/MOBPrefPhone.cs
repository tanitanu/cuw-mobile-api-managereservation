using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable]
    public class MOBPrefPhone
    {
        private long customerId;
        private string channelCode = string.Empty;
        private string channelCodeDescription = string.Empty;
        private int channelTypeSeqNum;
        private string channelTypeCode = string.Empty;
        private string channelTypeDescription = string.Empty;
        private string key = string.Empty;
        private string countryPhoneNumber = string.Empty;
        private string countryName = string.Empty;
        private string countryCode = string.Empty;
        private string areaNumber = string.Empty;
        private string phoneNumber = string.Empty;
        private string extensionNumber = string.Empty;
        private string description = string.Empty;
        private string languageCode = string.Empty;
        private bool isPrivate;
        private bool isNew;
        private bool isDefault;
        private bool isPrimary;
        private bool isSelected;
        private bool isProfileOwner;

        public long CustomerId
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

        public string ChannelCode
        {
            get
            {
                return this.channelCode;
            }
            set
            {
                this.channelCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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

        public int ChannelTypeSeqNum
        {
            get
            {
                return this.channelTypeSeqNum;
            }
            set
            {
                this.channelTypeSeqNum = value;
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
                this.channelTypeCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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

        public string CountryName
        {
            get
            {
                return this.countryName;
            }
            set
            {
                this.countryName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string LanguageCode
        {
            get
            {
                return this.languageCode;
            }
            set
            {
                this.languageCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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

        public bool IsNew
        {
            get
            {
                return this.isNew;
            }
            set
            {
                this.isNew = value;
            }
        }

        public bool IsDefault
        {
            get
            {
                return this.isDefault;
            }
            set
            {
                this.isDefault = value;
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

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
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
    }
}
