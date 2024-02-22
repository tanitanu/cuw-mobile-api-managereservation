using System;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBDOTBaggageLoyalty
    {
        public string ProgramId { get; set; } = string.Empty;

        public string MemberShipId { get; set; } = string.Empty;

        public string LoyalLevel { get; set; } = string.Empty;
    }
}
