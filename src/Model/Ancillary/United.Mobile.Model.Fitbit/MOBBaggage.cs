using System;

namespace United.Mobile.Model.Fitbit
{
    [Serializable]
    public class MOBBaggage
    {
        public string BagTerminal { get; set; } = string.Empty;

        public string BagClaimUnit { get; set; } = string.Empty;

        public bool HasBagLocation { get; set; }

    }
}
