using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Fitbit
{
    [Serializable]
    public class MileagePlusPNRResponse : MOBResponse
    {
        public string MileagePlusNumber { get; set; } = string.Empty;
        public List<PNR> PNRs { get; set; }
        public MileagePlusPNRResponse()
        {
            PNRs = new List<PNR>();
        }
    }
}
