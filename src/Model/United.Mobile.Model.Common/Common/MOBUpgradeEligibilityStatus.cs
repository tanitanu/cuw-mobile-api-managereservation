using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace United.Mobile.Model.Common
{
    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    //[JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumMemberConverter))]
    public enum MOBUpgradeEligibilityStatus
    {
        [EnumMember(Value = "0")]
        Unknown = 0,
        [EnumMember(Value = "1")]
        NotQualified = 1,
        [EnumMember(Value = "2")]
        Qualified = 2,
        [EnumMember(Value = "3")]
        Requested = 3,
        [EnumMember(Value = "4")]
        Upgraded = 4,
        [EnumMember(Value = "5")]
        NotUpgraded = 5,
        [EnumMember(Value = "6")]
        RequestConfirmed = 6,
    }
}
