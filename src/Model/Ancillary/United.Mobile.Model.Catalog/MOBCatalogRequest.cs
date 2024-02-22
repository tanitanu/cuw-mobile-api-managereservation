using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Catalog
{
    [Serializable()]
    public class MOBCatalogRequest : MOBRequest
    {
        public string MileagePlusNumber { get; set; } = string.Empty;
        public string HashPinCode { get; set; } = string.Empty;
    }
}
