using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.BagCalculator
{
    [Serializable]
    public class DOTBaggageLoyalty
    {
        public string ProgramId { get; set; } = string.Empty;
        public string MemberShipId { get; set; } = string.Empty;
        public string LoyalLevel { get; set; } = string.Empty;

    }
}
