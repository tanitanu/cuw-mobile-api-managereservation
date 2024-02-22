using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class FenceEvent
    {
        private int venueId;
        private string venueName = string.Empty;
        private string airportCode = string.Empty;
        private float venueLatitude;
        private float venueLongitude;
        private int fenceId;
        private string  fenceIdentifier = string.Empty;
        private float fenceLatitude;
        private float fenceLongitude;
        private float fenceRadius;
        private int eventId;
        private string eventName = string.Empty; 
        private string eventAction = string.Empty;
        private string fenceEventGuid = string.Empty;

        public int VenueId
        {
            get
            {
                return this.venueId;
            }
            set
            {
                this.venueId = value;
            }
        }

        public string VenueName
        {
            get
            {
                return this.venueName;
            }
            set
            {
                this.venueName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string AirportCode
        {
            get
            {
                return this.airportCode;
            }
            set
            {
                this.airportCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public float VenueLatitude
        {
            get
            {
                return this.venueLatitude;
            }
            set
            {
                this.venueLatitude = value;
            }
        }

        public float VenueLongitude
        {
            get
            {
                return this.venueLongitude;
            }
            set
            {
                this.venueLongitude = value;
            }
        }

        public int FenceId
        {
            get
            {
                return this.fenceId;
            }
            set
            {
                this.fenceId = value;
            }
        }

        public string FenceIdentifier
        {
            get
            {
                return this.fenceIdentifier;
            }
            set
            {
                this.fenceIdentifier = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public float FenceLatitude
        {
            get
            {
                return this.fenceLatitude;
            }
            set
            {
                this.fenceLatitude = value;
            }
        }

        public float FenceLongitude
        {
            get
            {
                return this.fenceLongitude;
            }
            set
            {
                this.fenceLongitude = value;
            }
        }

        public float FenceRadius
        {
            get
            {
                return this.fenceRadius;
            }
            set
            {
                this.fenceRadius = value;
            }
        }

        public int EventId
        {
            get
            {
                return this.eventId;
            }
            set
            {
                this.eventId = value;
            }
        }

        public string EventName
        {
            get
            {
                return this.eventName;
            }
            set
            {
                this.eventName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EventAction
        {
            get
            {
                return this.eventAction;
            }
            set
            {
                this.eventAction = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FenceEventGuid
        {
            get
            {
                return this.fenceEventGuid;
            }
            set
            {
                this.fenceEventGuid = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
