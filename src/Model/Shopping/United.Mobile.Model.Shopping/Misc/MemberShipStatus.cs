using System;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class MemberShipStatus
    {

        public string LoyaltyTypeLevelCode { get; set; } = string.Empty;

        public string LoyaltyTypeLevelDesc { get; set; } = string.Empty;

        public string GroupCode { get; set; } = string.Empty;

        public string GroupName { get; set; } = string.Empty;
        public MemberShipStatus()
        {

        }
        public MemberShipStatus(string loyalty_Type_Level_Code, string loyalty_Type_Level_Desc, string group_code, string group_name)
        {
            LoyaltyTypeLevelCode = loyalty_Type_Level_Code;
            LoyaltyTypeLevelDesc = loyalty_Type_Level_Desc;
            GroupCode = group_code;
            GroupName = group_name;
        }

    }
}
