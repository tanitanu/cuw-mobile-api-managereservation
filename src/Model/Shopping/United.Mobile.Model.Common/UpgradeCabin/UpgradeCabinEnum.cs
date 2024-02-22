using System;
using System.Runtime.Serialization;

namespace United.Mobile.Model.Shopping.UpgradeCabin
{
    [Serializable()]
    public enum UpgradeCabinUpgradeType
    {
        [EnumMember(Value = "PRICES")] //PCU
        PRICES = 0,
        [EnumMember(Value = "POINTS")] //UGC or CUG
        POINTS = 1,
        [EnumMember(Value = "MILES")] //MUA
        MILES = 2,       
    }

    [Serializable()]
    public enum UpgradeCabinOtherType
    {
        [EnumMember(Value = "COPAY")]
        COPAY = 0,
        [EnumMember(Value = "TAX")]
        TAX = 1,
    }

    [Serializable()]
    public enum UpgradeCabinPriceType
    {
        [EnumMember(Value = "PCU")] //PCU
        PRICES = 0,
        [EnumMember(Value = "UGC")] //UGC or CUG
        POINTS = 1,
        [EnumMember(Value = "MUA")] //MUA
        MILES = 2,       
    }
}
