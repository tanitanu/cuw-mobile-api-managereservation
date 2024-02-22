using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Catalog
{
    [Serializable()]
    public class MOBABTestingRequest : MOBRequest
    {
        public string MPAccountNumber { get; set; } = string.Empty;
    }
}
