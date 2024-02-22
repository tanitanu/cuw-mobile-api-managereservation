using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace United.Mobile.Model.FlightSearchResult
{
    public class FlightPBTRequest
    {  
        [Required]
        public string Origin { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        public string FlightNumber { get; set; }
        [Required]
        public string FlightDate { get; set; }
        [Required]
        public string CarrierCode { get; set; }       
        public string SessionId { get; set; }
    }
}
