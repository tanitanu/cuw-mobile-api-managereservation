using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Fitbit
{
    [Serializable()]
    public class MOBBundlesMerchandizingResponse : MOBResponse
    {
        public MOBBundlesMerchangdizingRequest Request { get; set; }

        public MOBBundleInfo BundleInfo { get; set; }
    }
}
