using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class BeaconEvent
    {
        private int venueId;
        private string venueName = string.Empty;
        private string airportCode = string.Empty;
        private float venueLatitude;
        private float venueLongitude;
        private int beaconId;
        private string beaconName = string.Empty; 
        private string beaconUuid = string.Empty;
        private int beaconMajor;
        private int beaconMinor;
        private float beaconLatitude;
        private float beaconLongitude;
        private int eventId; 
        private string eventName = string.Empty;
        private string beaconAction = string.Empty;
        private string fenceAction = string.Empty;
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

        public int BeaconId
        {
            get
            {
                return this.beaconId;
            }
            set
            {
                this.beaconId = value;
            }
        }

        public string BeaconName
        {
            get
            {
                return this.beaconName;
            }
            set
            {
                this.beaconName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string BeaconUuid
        {
            get
            {
                return this.beaconUuid;
            }
            set
            {
                this.beaconUuid = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public int BeaconMajor
        {
            get
            {
                return this.beaconMajor;
            }
            set
            {
                this.beaconMajor = value;
            }
        }

        public int BeaconMinor
        {
            get
            {
                return this.beaconMinor;
            }
            set
            {
                this.beaconMinor = value;
            }
        }

        public float BeaconLatitude
        {
            get
            {
                return this.beaconLatitude;
            }
            set
            {
                this.beaconLatitude = value;
            }
        }

        public float BeaconLongitude
        {
            get
            {
                return this.beaconLongitude;
            }
            set
            {
                this.beaconLongitude = value;
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
                this.eventName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string BeaconAction
        {
            get
            {
                return this.beaconAction;
            }
            set
            {
                this.beaconAction = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FenceAction
        {
            get
            {
                return this.fenceAction;
            }
            set
            {
                this.fenceAction = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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
