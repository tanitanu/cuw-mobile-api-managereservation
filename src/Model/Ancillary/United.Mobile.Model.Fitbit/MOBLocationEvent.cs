using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Fitbit
{
    [Serializable]
    public class MOBLocationEvent
    {
        public string MileagePlusNumber { get; set; } = string.Empty;

        public string RecordLocator { get; set; } = string.Empty;


        public string Origin { get; set; } = string.Empty;


        public string Destination { get; set; } = string.Empty;


        public List<MOBFenceEvent> FenceEvents { get; set; }


        public List<MOBBeaconEvent> BeaconEvents { get; set; }

        public MOBLocationEvent()
        {
            FenceEvents = new List<MOBFenceEvent>();
            BeaconEvents = new List<MOBBeaconEvent>();
        }
    }
}
