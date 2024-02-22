using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Fitbit
{
    [Serializable()]
    public class MOBBundlesMerchangdizingRequest : MOBRequest
    {
        public string RecordLocator { get; set; } = string.Empty;
    }
}
