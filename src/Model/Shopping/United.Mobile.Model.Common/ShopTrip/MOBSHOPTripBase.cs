using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBSHOPTripBase
    {
        private string departDate = string.Empty;
        private string arrivalDate;
        private string shareMessage = string.Empty;

        private string origin = string.Empty;

        //MB-2639 add all airports flag
        private int originAllAirports = -1;

        private string destination = string.Empty;

        //MB-2639 add all airports flag
        private int destinationAllAirports = -1;

        private string cabin = string.Empty;
        private bool useFilters = false;
        private MOBSearchFilters searchFiltersIn;
        private MOBSearchFilters searchFiltersOut;

        private bool searchNearbyOriginAirports = false;
        private bool searchNearbyDestinationAirports = false;
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

        [JsonPropertyName("originAllAirports")]
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
                this.arrivalDate = value; //string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public MOBSearchFilters SearchFiltersIn
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

        public MOBSearchFilters SearchFiltersOut
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
        public string ShareMessage
        {
            get
            {
                return this.shareMessage;
            }
            set
            {
                this.shareMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

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
    //[System.Text.Json.Serialization.JsonConverter(typeof(StringEnumConverter))]
    public enum MOBSHOPTripChangeType
    {
        [EnumMember(Value = "0")]
        [Display(Name = "0")]
        ChangeFlight,
        [EnumMember(Value = "1")]
        [Display(Name = "1")]
        AddFlight,
        [EnumMember(Value = "2")]
        [Display(Name = "2")]
        DeleteFlight,
        [EnumMember(Value = "3")]
        [Display(Name = "3")]
        NoChange
    }
}
