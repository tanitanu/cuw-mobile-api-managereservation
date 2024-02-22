using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.MPRewards
{
    [Serializable()]
    public class MOBCancelFFCPNRsByMPNumberResponse : MOBResponse
    {
        public bool FutureFlightCreditLink { get; set; } 
        public List<MOBCancelledFFCPNRDetails> CancelledFFCPNRList { get; set; } 
        public string MileagePlusNumber { get; set; } = string.Empty;
    }
}
