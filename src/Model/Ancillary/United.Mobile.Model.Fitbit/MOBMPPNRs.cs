using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Fitbit
{
    [Serializable()]
    public class MOBMPPNRs
    {
        public string MileagePlusNumber { get; set; } = string.Empty;

        public List<MOBPNR> Current { get; set; } 

        public List<MOBPNR> Past { get; set; } 

        public List<MOBPNR> Cancelled { get; set; } 

        public List<MOBPNR> Inactive { get; set; } 

        public bool GotException4GetPNRSbyMPCSLcallDoNotDropExistingPnrsInWallet { get; set; } 
    }
}
