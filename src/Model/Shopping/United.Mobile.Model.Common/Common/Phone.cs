using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class Phone
    {
        public string AreaNumber { get; set; } = string.Empty;

        public string Attention { get; set; } = string.Empty;

        public string ChannelCode { get; set; } = string.Empty;

        public string ChannelCodeDescription { get; set; } = string.Empty;

        public string ChannelTypeCode { get; set; } = string.Empty;

        public string ChannelTypeDescription { get; set; } = string.Empty;

        public int ChannelTypeSeqNumber { get; set; }

        public string CountryCode { get; set; } = string.Empty;
        public string CountryNumber { get; set; } = string.Empty;
        public string CountryPhoneNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string DiscontinuedDate { get; set; } = string.Empty;
        public string EffectiveDate { get; set; } = string.Empty;

        public string ExtensionNumber { get; set; } = string.Empty;

        public bool IsPrimary { get; set; }

        public bool IsPrivate { get; set; }
        public bool IsProfileOwner { get; set; }
        public string Key { get; set; } = string.Empty;

        public string LanguageCode { get; set; } = string.Empty;

        public string MileagePlusId { get; set; } = string.Empty;

        public string PagerPinNumber { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string SharesCountryCode { get; set; } = string.Empty;

        public string WrongPhoneDate { get; set; } = string.Empty;

    }
}
