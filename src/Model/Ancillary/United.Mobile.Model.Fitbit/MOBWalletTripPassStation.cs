using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Fitbit
{
    [Serializable]
    public class MOBWalletTripPassStation
    {
        private string stationCode = string.Empty;
        private string stationName = string.Empty;
        private bool hasUnitedClub;

        public string StationCode
        {
            get
            {
                return this.stationCode;
            }
            set
            {
                this.stationCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string StationName
        {
            get
            {
                return this.stationName;
            }
            set
            {
                this.stationName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool HasUnitedClub
        {
            get
            {
                return this.hasUnitedClub;
            }
            set
            {
                this.hasUnitedClub = value;
            }
        }

    }
}
