using System;

namespace United.Mobile.Model.Fitbit
{
    [Serializable]
    public class MOBFenceEvent
    {
        public int VenueId { get; set; }

        public string VenueName { get; set; } = string.Empty;

        public string AirportCode { get; set; } = string.Empty;

        public float VenueLatitude { get; set; }

        public float VenueLongitude { get; set; }

        public int FenceId { get; set; }

        public string FenceIdentifier { get; set; } = string.Empty;

        public float FenceLatitude { get; set; }

        public float FenceLongitude { get; set; }

        public float FenceRadius { get; set; }

        public int EventId { get; set; }

        public string EventName { get; set; } = string.Empty;

        public string EventAction { get; set; } = string.Empty;

        public string FenceEventGuid { get; set; } = string.Empty;
    }
}
