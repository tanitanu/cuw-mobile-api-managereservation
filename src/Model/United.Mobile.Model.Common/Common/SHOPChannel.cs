using System;
using System.Xml.Serialization;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    [XmlRoot("MOBChannel")]
    public class SHOPChannel
    {
        public string ChannelCode { get; set; } = string.Empty;
        public string ChannelDescription { get; set; } = string.Empty;
        public string ChannelTypeCode { get; set; } = string.Empty;
        public string ChannelTypeDescription { get; set; } = string.Empty;
    }
}

