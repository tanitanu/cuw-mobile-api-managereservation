using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Fitbit
{
    [Serializable]
    public class MOBWalletTripPass
    {
        private string id = string.Empty;
        private string barcode = string.Empty;
        private string bundleType = string.Empty;
        private string bundleTypeDesc = string.Empty;
        private string bundleRoute = string.Empty;
        private List<MOBWalletTripPassStation> stations;
        private string expirationDate = string.Empty;

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Barcode
        {
            get
            {
                return this.barcode;
            }
            set
            {
                this.barcode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string BundleType
        {
            get
            {
                return this.bundleType;
            }
            set
            {
                this.bundleType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string BundleTypeDesc
        {
            get
            {
                return this.bundleTypeDesc;
            }
            set
            {
                this.bundleTypeDesc = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string BundleRoute
        {
            get
            {
                return this.bundleRoute;
            }
            set
            {
                this.bundleRoute = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBWalletTripPassStation> Stations
        {
            get
            {
                return this.stations;
            }
            set
            {
                this.stations = value;
            }
        }

        public string ExpirationDate
        {
            get
            {
                return this.expirationDate;
            }
            set
            {
                this.expirationDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
