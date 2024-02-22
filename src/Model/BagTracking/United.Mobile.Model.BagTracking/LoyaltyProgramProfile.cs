using System;

namespace United.Mobile.Model.BagTracking
{
    [Serializable]
    public class LoyaltyProgramProfile
    {
        public string LoyaltyProgramCarrierCode { get; set; } = string.Empty;
        public string LoyaltyProgramID { get; set; } = string.Empty;
        public string LoyaltyProgramMemberID { get; set; } = string.Empty;

    }
}
