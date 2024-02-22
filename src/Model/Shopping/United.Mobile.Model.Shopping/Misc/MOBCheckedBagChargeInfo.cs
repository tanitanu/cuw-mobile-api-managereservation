using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class CheckedBagChargeInfo
    {
        private List<MOBItem> captions;
        private List<BagFeesPerSegment> bagFeeSegments;
        private string cartId;
        private MemberShipStatus loyaltyLevelSelected;
        private List<MemberShipStatus> loyaltyLevels;

        public List<MOBItem> Captions
        {
            get { return captions; }
            set { captions = value; }
        }

        public List<BagFeesPerSegment> BagFeeSegments
        {
            get { return bagFeeSegments; }
            set { bagFeeSegments = value; }
        }

        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }

        public MemberShipStatus LoyaltyLevelSelected
        {
            get { return loyaltyLevelSelected; }
            set { loyaltyLevelSelected = value; }
        }

        public List<MemberShipStatus> LoyaltyLevels
        {
            get { return loyaltyLevels; }
            set { loyaltyLevels = value; }
        }
    }
}
