using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBBundleTraveler : MOBDOTBaggageTravelerInfo
    {
        public string BundleDescription { get; set; } = string.Empty;
    }
}
