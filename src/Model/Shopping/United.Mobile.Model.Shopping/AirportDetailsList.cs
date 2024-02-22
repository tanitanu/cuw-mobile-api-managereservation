using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class AirportDetailsList
    {
        public List<MOBDisplayBagTrackAirportDetails> AirportsList { get; set; }
        public string ObjectName { get; set; } = "United.Persist.Definition.Shopping.AirportDetailsList";
    }
}
