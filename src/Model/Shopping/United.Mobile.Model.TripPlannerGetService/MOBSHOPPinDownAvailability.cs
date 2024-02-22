using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace United.Mobile.Model.TripPlannerGetService
{
    [Serializable]
    public class MOBSHOPPinDownAvailability
    {
        private string cabin = string.Empty;
        private string vendorName = string.Empty;
        private string vendorGUID = string.Empty;
        private string searchBy = string.Empty;
        private string countryCode = string.Empty;
        private List<MOBSHOPPinDownTrip> trips;
        private List<MOBSHOPPinDownPTC> ptcs;
        private string searchType = string.Empty;

        [XmlAttribute]
        public string Cabin
        {
            get
            {
                return cabin;
            }
            set
            {
                this.cabin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        [XmlAttribute]
        public string VendorName
        {
            get
            {
                return this.vendorName;
            }
            set
            {
                this.vendorName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        [XmlAttribute]
        public string VendorGUID
        {
            get
            {
                return vendorGUID;
            }
            set
            {
                this.vendorGUID = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        [XmlAttribute]
        public string SearchBy
        {
            get
            {
                return this.searchBy;
            }
            set
            {
                this.searchBy = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        [XmlAttribute]
        public string CountryCode
        {
            get
            {
                return this.countryCode;
            }
            set
            {
                this.countryCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public List<MOBSHOPPinDownTrip> Trips
        {
            get
            {
                return this.trips;
            }
            set
            {
                this.trips = value;
            }
        }

        public List<MOBSHOPPinDownPTC> PTCs
        {
            get
            {
                return this.ptcs;
            }
            set
            {
                this.ptcs = value;
            }
        }

        [XmlAttribute]
        public string SearchType
        {
            get
            {
                return searchType;
            }
            set
            {
                this.searchType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
