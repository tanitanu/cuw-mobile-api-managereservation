using System;
using System.Runtime.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{

    [Serializable]
    public class GetProductInfoForFSRDRequest : MOBRequest
    {
        
        public string SessionId { get; set; } = string.Empty;
      
        /// <example>
        /// "US"
        /// </example>
        public string CountryCode { get; set; } = string.Empty;
        

        /// <example>
        /// "16-31|1180-UA"
        /// </example>
        /// <hint>
        /// Given in the Shop Response
        /// </hint>
        public string FlightHash { get; set; } = string.Empty;
       
        public string SearchType { get; set; } = string.Empty;
      
        public string TripId { get; set; } = string.Empty;
       
        public int NumberOfAdults { get; set; } 

        public bool IsIBE { get; set; } 

    }

    
}
