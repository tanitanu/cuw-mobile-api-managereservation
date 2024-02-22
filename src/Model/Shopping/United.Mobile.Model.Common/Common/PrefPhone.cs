using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class PrefPhone
    {
        public long CustomerId { get; set; }

        public string ChannelCode { get; set; } = string.Empty;

        public string ChannelCodeDescription { get; set; } = string.Empty;

        public int ChannelTypeSeqNum { get; set; }
        public string ChannelTypeCode { get; set; } = string.Empty;

        public string ChannelTypeDescription { get; set; } = string.Empty;

        public string Key { get; set; } = string.Empty;

        public string CountryPhoneNumber { get; set; } = string.Empty;

        public string CountryName { get; set; } = string.Empty;

        public string CountryCode { get; set; } = string.Empty;

        public string AreaNumber { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string ExtensionNumber { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string LanguageCode { get; set; } = string.Empty;

        public bool IsPrivate { get; set; }

        public bool IsNew { get; set; }

        public bool IsDefault { get; set; }

        public bool IsPrimary { get; set; }
        public bool IsSelected { get; set; }

        public bool IsProfileOwner { get; set; }
    }
}
