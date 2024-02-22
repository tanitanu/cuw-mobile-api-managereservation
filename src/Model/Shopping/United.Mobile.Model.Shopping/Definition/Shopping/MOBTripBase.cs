using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace United.Persist.Definition.Shopping
{
    [Serializable()]
    public class MOBTripBase
    {
        private string origin = string.Empty;

        //MB-2639 add all airports flag
        private int originAllAirports = -1;

        private string destination = string.Empty;

        //MB-2639 add all airports flag
        private int destinationAllAirports = -1;

        private string departDate = string.Empty;
        private string arrivalDate = string.Empty;
        private string cabin = string.Empty;
        private bool useFilters = false;
        private MOBSHOPSearchFilters searchFiltersIn { get; set; }
        private MOBSHOPSearchFilters searchFiltersOut { get; set; }

        private bool searchNearbyOriginAirports = false;
        private bool searchNearbyDestinationAirports = false;
        private string shareMessage = string.Empty;
        private int index;


        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
            }
        }
        public string Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public int OriginAllAirports
        {
            get
            {
                return originAllAirports;
            }
            set
            {
                this.originAllAirports = value;
            }
        }

        public string Destination
        {
            get
            {
                return this.destination;
            }
            set
            {
                this.destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
        public int DestinationAllAirports
        {
            get
            {
                return destinationAllAirports;
            }
            set
            {
                this.destinationAllAirports = value;
            }
        }

        public string DepartDate
        {
            get
            {
                return this.departDate;
            }
            set
            {
                this.departDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ArrivalDate
        {
            get
            {
                return this.arrivalDate;
            }
            set
            {
                this.arrivalDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Cabin
        {
            get
            {
                return this.cabin;
            }
            set
            {
                this.cabin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool UseFilters
        {
            get
            {
                return this.useFilters;
            }
            set
            {
                this.useFilters = value;
            }
        }

        public MOBSHOPSearchFilters SearchFiltersIn
        {
            get
            {
                return this.searchFiltersIn;
            }
            set
            {
                this.searchFiltersIn = value;
            }
        }

        public MOBSHOPSearchFilters SearchFiltersOut
        {
            get
            {
                return this.searchFiltersOut;
            }
            set
            {
                this.searchFiltersOut = value;
            }
        }

        public bool SearchNearbyOriginAirports
        {
            get
            {
                return this.searchNearbyOriginAirports;
            }
            set
            {
                this.searchNearbyOriginAirports = value;
            }
        }

        public bool SearchNearbyDestinationAirports
        {
            get
            {
                return this.searchNearbyDestinationAirports;
            }
            set
            {
                this.searchNearbyDestinationAirports = value;
            }
        }
        public string ShareMessage { get; set; } = string.Empty;

        private MOBSHOPTripChangeType changeType = MOBSHOPTripChangeType.NoChange;
        public MOBSHOPTripChangeType ChangeType
        {
            get
            {
                return this.changeType;
            }
            set
            {
                this.changeType = value;
            }
        }
    }

    [Serializable]
    public enum MOBSHOPTripChangeType
    {
        [EnumMember(Value = "0")]
        ChangeFlight,
        [EnumMember(Value = "1")]
        AddFlight,
        [EnumMember(Value = "2")]
        DeleteFlight,
        [EnumMember(Value = "3")]
        NoChange
    }
}

