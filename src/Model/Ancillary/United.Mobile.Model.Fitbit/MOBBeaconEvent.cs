using System;

namespace United.Mobile.Model.Fitbit
{
    [Serializable]
    public class MOBBeaconEvent
    {
        public int VenueId { get; set; }

        public string VenueName { get; set; } = string.Empty;

        public string AirportCode { get; set; } = string.Empty;

        public float VenueLatitude { get; set; }

        public float VenueLongitude { get; set; }

        public int BeaconId { get; set; }

        public string BeaconName { get; set; } = string.Empty;

        public string BeaconUuid { get; set; } = string.Empty;

        public int BeaconMajor { get; set; }

        public int BeaconMinor { get; set; }

        public float BeaconLatitude { get; set; }

        public float BeaconLongitude { get; set; }

        public int EventId { get; set; }

        public string EventName { get; set; } = string.Empty;

        public string BeaconAction { get; set; } = string.Empty;

        public string FenceAction { get; set; } = string.Empty;

        public string FenceEventGuid { get; set; } = string.Empty;
    }
}
