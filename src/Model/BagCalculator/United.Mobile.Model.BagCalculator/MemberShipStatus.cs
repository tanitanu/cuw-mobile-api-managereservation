using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.BagCalculator
{
    [Serializable()]
    public class MemberShipStatus
    {
        public string LoyaltyTypeLevelCode { get; set; }
        public string LoyaltyTypeLevelDesc { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
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
