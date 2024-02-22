using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class LocationEventResult
    {
        private string mileagePlusNumber = string.Empty;
        private string recordLocator = string.Empty;
        private string locationEventType = string.Empty;
        private string locationEventTriggerType = string.Empty;
        private string fenceIdentifier = string.Empty;
        private string fenceAirportCode = string.Empty;
        private string beaconUuid = string.Empty;
        private int beaconMajor;
        private int beaconMinor;
        private string beaconProximity;
        private string timestamp = string.Empty;


        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string RecordLocator
        {
            get
            {
                return this.recordLocator;
            }
            set
            {
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string LocationEventType
        {
            get
            {
                return this.locationEventType;
            }
            set
            {
                this.locationEventType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string LocationEventTriggerType
        {
            get
            {
                return this.locationEventTriggerType;
            }
            set
            {
                this.locationEventTriggerType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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

        public string FenceAirportCode
        {
            get
            {
                return this.fenceAirportCode;
            }
            set
            {
                this.fenceAirportCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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

        public string BeaconProximity
        {
            get
            {
                return this.beaconProximity;
            }
            set
            {
                this.beaconProximity = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Timestamp
        {
            get
            {
                return this.timestamp;
            }
            set
            {
                this.timestamp = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
