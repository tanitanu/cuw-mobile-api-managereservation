using System;
using System.Runtime.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class MOBSHOPGetProductInfoForFSRDRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string countryCode = "US";
        private string flightHash = string.Empty;
        private string searchType = string.Empty;
        private string tripId = string.Empty;
        private int numberOfAdults = 0;
        private bool isIBE;

        public string SessionId
        {
            get
            {
                return sessionId;
            }
            set
            {
                sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        /// <example>
        /// "US"
        /// </example>
        public string CountryCode
        {
            get
            {
                return countryCode;
            }
            set
            {
                countryCode = string.IsNullOrEmpty(value) ? "US" : value.Trim().ToUpper();
            }
        }

        /// <example>
        /// "16-31|1180-UA"
        /// </example>
        /// <hint>
        /// Given in the Shop Response
        /// </hint>
        public string FlightHash
        {
            get
            {
                return flightHash;
            }
            set
            {
                flightHash = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        
        public string SearchType
        {
            get { return searchType; }
            set { searchType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string TripId
        {
            get { return tripId; }
            set { tripId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public int NumberOfAdults
        {
            get { return numberOfAdults; }
            set { numberOfAdults = value; }
        }

        public bool IsIBE
        {
            get { return isIBE; }
            set { isIBE = value; }
        }

    }

    
}
