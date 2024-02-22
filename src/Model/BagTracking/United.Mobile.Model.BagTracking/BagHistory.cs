using System;

namespace United.Mobile.Model.BagTracking
{
    [Serializable]
    public class BagHistory
    {
        public BagFlightSegment BagFlightSegment { get; set; }

        public string StatusCode { get; set; } = string.Empty;

    }
}
