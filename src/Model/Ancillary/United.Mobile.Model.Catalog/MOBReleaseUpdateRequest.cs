using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Catalog
{
    [Serializable()]
    public class MOBReleaseUpdateRequest : MOBRequest
    {
        public string OSVersion { get; set; } = string.Empty;
        public int OSVersionCounter { get; set; }
        public int AppVersionCounter { get; set; }
        public string MileagePlusID { get; set; } = string.Empty;
        public string HashKey { get; set; } = string.Empty;
        public string CustID { get; set; } = string.Empty;
    }
}
