using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBBundle
    {
        public string PassengerSharesPosition { get; set; } = string.Empty;

        public string SegmentId { get; set; } = string.Empty;

        public string Bundle { get; set; } = string.Empty;

        public bool IsEPU { get; set; }

        public bool IsPremierAccess { get; set; }

        public bool IsBonusMiles { get; set; }

        public bool IsClubTripPass { get; set; }

        public bool IsExtraBag { get; set; }

        public string BundleDescription { get; set; } = string.Empty;
    }
}
