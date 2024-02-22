using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBBundleFlightSegment : MOBFlightSegment
    {
        public string Bundle { get; set; } = string.Empty;

        public bool IsEPU { get; set; }

        public bool IsPremierAccess { get; set; }

        public bool IsBonusMiles { get; set; }

        public bool IsClubTripPass { get; set; }

        public bool IsExtraBag { get; set; }

        public string SegmentId { get; set; } = string.Empty;

        public List<MOBBundleTraveler> Travelers { get; set; }

        public MOBBundleFlightSegment()
        {
            Travelers = new List<MOBBundleTraveler>();
        }

    }
}
