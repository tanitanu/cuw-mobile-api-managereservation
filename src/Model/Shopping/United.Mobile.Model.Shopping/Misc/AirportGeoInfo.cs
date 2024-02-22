using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class AirportGeoInfo
    {
        private string airportCode = string.Empty;
        private string airportDescription = string.Empty;
        private string stateCode = string.Empty;
        private string countryCode = string.Empty;
        private string latitude = string.Empty;
        private string longitude = string.Empty;
        private string distance = string.Empty;

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

        public string AirportDescription
        {
            get
            {
                return this.airportDescription;
            }
            set
            {
                this.airportDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string StateCode
        {
            get
            {
                return this.stateCode;
            }
            set
            {
                this.stateCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

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

        public string Latitude
        {
            get
            {
                return this.latitude;
            }
            set
            {
                this.latitude = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Longitude
        {
            get
            {
                return this.longitude;
            }
            set
            {
                this.longitude = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Distance
        {
            get
            {
                return this.distance;
            }
            set
            {
                this.distance = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
