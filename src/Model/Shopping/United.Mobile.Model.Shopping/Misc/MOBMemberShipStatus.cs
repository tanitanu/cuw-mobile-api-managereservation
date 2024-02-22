using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MemberShipStatus
    {
        private string loyaltyTypeLevelCode = string.Empty;
        private string loyaltyTypeLevelDesc = string.Empty;
        private string groupCode = string.Empty;
        private string groupName = string.Empty;

        public MemberShipStatus()
        {

        }
        public MemberShipStatus(string loyalty_Type_Level_Code, string loyalty_Type_Level_Desc, string group_code, string group_name)
        {
            loyaltyTypeLevelCode = loyalty_Type_Level_Code;
            loyaltyTypeLevelDesc = loyalty_Type_Level_Desc;
            groupCode = group_code;
            groupName = group_name;
        }

        public string LoyaltyTypeLevelCode
        {
            get
            {
                return this.loyaltyTypeLevelCode;
            }
            set
            {
                this.loyaltyTypeLevelCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string LoyaltyTypeLevelDesc
        {
            get
            {
                return this.loyaltyTypeLevelDesc;
            }
            set
            {
                this.loyaltyTypeLevelDesc = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string GroupCode
        {
            get
            {
                return this.groupCode;
            }
            set
            {
                this.groupCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string GroupName
        {
            get
            {
                return this.groupName;
            }
            set
            {
                this.groupName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
