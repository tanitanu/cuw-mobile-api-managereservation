using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public enum UpgradeEligibilityStatus
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
