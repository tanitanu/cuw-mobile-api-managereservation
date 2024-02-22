﻿using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class CheckedBagChargeInfo
    {
        public List<MOBItem> Captions { get; set; }
        public List<BagFeesPerSegment> BagFeeSegments { get; set; }
        public string CartId { get; set; } = string.Empty;

        public MemberShipStatus LoyaltyLevelSelected { get; set; }
        public List<MemberShipStatus> LoyaltyLevels { get; set; }
        public CheckedBagChargeInfo()
        {
            Captions = new List<MOBItem>();
            BagFeeSegments = new List<BagFeesPerSegment>();
            LoyaltyLevels = new List<MemberShipStatus>();
        }
    }
}
