using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class CheckedBagInfoResponse //:MOBResponse
    {
      
        public CheckedBagInfoRequest Request { get; set; }
       
        public CheckedBagChargeInfo CheckedBagChargeInfo { get; set; }

        public string SessionId { get; set; } = string.Empty;

        public string CartId { get; set; } = string.Empty;
       
        public MOBSHOPResponseStatusItem ResponseStatusItem { get; set; }

    }
}
