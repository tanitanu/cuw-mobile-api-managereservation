using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class BaggageCalculatorSearchResponse : MOBResponse
    {
        public BaggageCalculatorSearchResponse()
            : base()
        {

        }

        public BaggageCalculatorSearchResponse(int applicationID)
            : base()
        {
            loyaltyLevels = new List<MemberShipStatus>();

            if (applicationID == 3)
            {
                #region
                loyaltyLevels.Add(new MemberShipStatus("GeneralMember", "General Member", "1", ""));

                loyaltyLevels.Add(new MemberShipStatus("PremierSilver", "Premier Silver", "2", "MileagePlus Premier® member"));

                loyaltyLevels.Add(new MemberShipStatus("PremierGold", "Premier Gold", "2", "MileagePlus Premier® member"));

                loyaltyLevels.Add(new MemberShipStatus("PremierPlatinum", "Premier Platinum", "2", "MileagePlus Premier® member"));

                loyaltyLevels.Add(new MemberShipStatus("Premier1K", "Premier 1K", "2", "MileagePlus Premier® member"));

                loyaltyLevels.Add(new MemberShipStatus("GlobalServices", "Global Services", "2", "MileagePlus Premier® member"));

                loyaltyLevels.Add(new MemberShipStatus("StarAllianceSilver", "Star Alliance Silver", "3", "Star Alliance status"));

                loyaltyLevels.Add(new MemberShipStatus("StarAllianceGold", "Star Alliance Gold", "3", "Star Alliance status"));

                loyaltyLevels.Add(new MemberShipStatus("PPC", "Presidental Plus Card", "4", "MileagePlus cardmember"));

                loyaltyLevels.Add(new MemberShipStatus("MEC", "MileagePlus Explorer Card", "4", "MileagePlus cardmember"));

                //loyaltyLevels.Add(new MemberShipStatus("OPC", "One Pass Club", "4", "MileagePlus cardmember"));

                loyaltyLevels.Add(new MemberShipStatus("CCC", "Chase Club Card", "4", "MileagePlus cardmember"));

                loyaltyLevels.Add(new MemberShipStatus("MIL", "Active U.S. military(leisure travel)", "5", "Active U.S. military"));

                loyaltyLevels.Add(new MemberShipStatus("MIR", "Active U.S. military(on duty)", "5", "Active U.S. military"));
                #endregion
            }
            else
            {
                #region
                loyaltyLevels.Add(new MemberShipStatus("GeneralMember", "General Member", "1", ""));

                loyaltyLevels.Add(new MemberShipStatus("PremierSilver", "Premier Silver member", "2", "MileagePlus Premier® member"));

                loyaltyLevels.Add(new MemberShipStatus("PremierGold", "Premier Gold member", "2", "MileagePlus Premier® member"));

                loyaltyLevels.Add(new MemberShipStatus("PremierPlatinum", "Premier Platinum member", "2", "MileagePlus Premier® member"));

                loyaltyLevels.Add(new MemberShipStatus("Premier1K", "Premier 1K member", "2", "MileagePlus Premier® member"));

                loyaltyLevels.Add(new MemberShipStatus("GlobalServices", "Global Services member", "2", "MileagePlus Premier® member"));

                loyaltyLevels.Add(new MemberShipStatus("StarAllianceGold", "Star Alliance Gold member", "3", "Star Alliance status"));

                loyaltyLevels.Add(new MemberShipStatus("StarAllianceSilver", "Star Alliance Silver member", "3", "Star Alliance status"));

                loyaltyLevels.Add(new MemberShipStatus("MEC", "MileagePlus Explorer Card member", "4", "MileagePlus cardmember"));

                //loyaltyLevels.Add(new MemberShipStatus("OPC", "OnePass Plus Card member", "4", "MileagePlus cardmember"));

                loyaltyLevels.Add(new MemberShipStatus("CCC", "MileagePlus Club Card member", "4", "MileagePlus cardmember"));

                loyaltyLevels.Add(new MemberShipStatus("PPC", "Presidental Plus Card member", "4", "MileagePlus cardmember"));

                loyaltyLevels.Add(new MemberShipStatus("MIR", "U.S. Military on orders or relocating", "5", "Active U.S. military"));

                loyaltyLevels.Add(new MemberShipStatus("MIL", "U.S. Military personal travel", "5", "Active U.S. military"));
                #endregion
            }
        }

        private List<MemberShipStatus> loyaltyLevels;


        private List<CarrierInfo> carriers;

        private List<ClassOfService> classOfServices;

        public List<MemberShipStatus> LoyaltyLevels
        {
            get
            {
                return this.loyaltyLevels;
            }
            set
            {
                this.loyaltyLevels = value;
            }
        }

        public List<CarrierInfo> Carriers
        {
            get
            {
                return this.carriers;
            }
            set
            {
                this.carriers = value;
            }
        }

        public List<ClassOfService> ClassOfServices
        {
            get
            {
                return this.classOfServices;
            }
            set
            {
                this.classOfServices = value;
            }
        }
    }
}
