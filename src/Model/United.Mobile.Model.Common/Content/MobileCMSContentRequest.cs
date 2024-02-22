using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Common
{
    public class MobileCMSContentRequest:MOBRequest
    {
        public MobileCMSContentRequest() : base() { }
        public string SessionId { get; set; } = string.Empty;
        public string CartId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string MileagePlusNumber { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public List<string> ListNames { get; set; } = new List<string>();
        public bool GetShopTnC { get; set; }
        public string Flow { get; set; } = string.Empty;
    }
}
