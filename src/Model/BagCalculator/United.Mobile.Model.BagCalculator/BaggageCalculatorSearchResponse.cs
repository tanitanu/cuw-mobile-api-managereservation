using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.BagCalculator
{
    [Serializable]
    public class BaggageCalculatorSearchResponse : MOBResponse
    {
        public List<MemberShipStatus> LoyaltyLevels { get; set; }
        public List<CarrierInfo> Carriers { get; set; }
        public List<ClassOfService> ClassOfServices { get; set; }

        public BaggageCalculatorSearchResponse()
        {
            LoyaltyLevels = new List<MemberShipStatus>();
            Carriers = new List<CarrierInfo>();
            ClassOfServices = new List<ClassOfService>();
        }
        public BaggageCalculatorSearchResponse(int applicationID)
            : base()
        {
            LoyaltyLevels = new List<MemberShipStatus>();

            if (applicationID == 3)
            {
                #region
                LoyaltyLevels.Add(new MemberShipStatus("GeneralMember", "General Member", "1", ""));

                LoyaltyLevels.Add(new MemberShipStatus("PremierSilver", "Premier Silver", "2", "MileagePlus Premier® member"));

                LoyaltyLevels.Add(new MemberShipStatus("PremierGold", "Premier Gold", "2", "MileagePlus Premier® member"));

                LoyaltyLevels.Add(new MemberShipStatus("PremierPlatinum", "Premier Platinum", "2", "MileagePlus Premier® member"));

                LoyaltyLevels.Add(new MemberShipStatus("Premier1K", "Premier 1K", "2", "MileagePlus Premier® member"));

                LoyaltyLevels.Add(new MemberShipStatus("GlobalServices", "Global Services", "2", "MileagePlus Premier® member"));

                LoyaltyLevels.Add(new MemberShipStatus("StarAllianceSilver", "Star Alliance Silver", "3", "Star Alliance status"));

                LoyaltyLevels.Add(new MemberShipStatus("StarAllianceGold", "Star Alliance Gold", "3", "Star Alliance status"));

                LoyaltyLevels.Add(new MemberShipStatus("PPC", "Presidental Plus Card", "4", "MileagePlus cardmember"));

                LoyaltyLevels.Add(new MemberShipStatus("MEC", "MileagePlus Explorer Card", "4", "MileagePlus cardmember"));

                //LoyaltyLevels.Add(new MemberShipStatus("OPC", "One Pass Club", "4", "MileagePlus cardmember"));

                LoyaltyLevels.Add(new MemberShipStatus("CCC", "Chase Club Card", "4", "MileagePlus cardmember"));

                LoyaltyLevels.Add(new MemberShipStatus("MIL", "Active U.S. military(leisure travel)", "5", "Active U.S. military"));

                LoyaltyLevels.Add(new MemberShipStatus("MIR", "Active U.S. military(on duty)", "5", "Active U.S. military"));
                #endregion
            }
            else
            {
                #region
                LoyaltyLevels.Add(new MemberShipStatus("GeneralMember", "General Member", "1", ""));

                LoyaltyLevels.Add(new MemberShipStatus("PremierSilver", "Premier Silver member", "2", "MileagePlus Premier® member"));

                LoyaltyLevels.Add(new MemberShipStatus("PremierGold", "Premier Gold member", "2", "MileagePlus Premier® member"));

                LoyaltyLevels.Add(new MemberShipStatus("PremierPlatinum", "Premier Platinum member", "2", "MileagePlus Premier® member"));

                LoyaltyLevels.Add(new MemberShipStatus("Premier1K", "Premier 1K member", "2", "MileagePlus Premier® member"));

                LoyaltyLevels.Add(new MemberShipStatus("GlobalServices", "Global Services member", "2", "MileagePlus Premier® member"));

                LoyaltyLevels.Add(new MemberShipStatus("StarAllianceGold", "Star Alliance Gold member", "3", "Star Alliance status"));

                LoyaltyLevels.Add(new MemberShipStatus("StarAllianceSilver", "Star Alliance Silver member", "3", "Star Alliance status"));

                LoyaltyLevels.Add(new MemberShipStatus("MEC", "MileagePlus Explorer Card member", "4", "MileagePlus cardmember"));

                //LoyaltyLevels.Add(new MemberShipStatus("OPC", "OnePass Plus Card member", "4", "MileagePlus cardmember"));

                LoyaltyLevels.Add(new MemberShipStatus("CCC", "MileagePlus Club Card member", "4", "MileagePlus cardmember"));

                LoyaltyLevels.Add(new MemberShipStatus("PPC", "Presidental Plus Card member", "4", "MileagePlus cardmember"));

                LoyaltyLevels.Add(new MemberShipStatus("MIR", "U.S. Military on orders or relocating", "5", "Active U.S. military"));

                LoyaltyLevels.Add(new MemberShipStatus("MIL", "U.S. Military personal travel", "5", "Active U.S. military"));
                #endregion
            }
        }
    }
}
