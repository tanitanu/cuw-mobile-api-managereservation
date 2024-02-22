using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.FlightSearchResult
{
    public class FlightSearchResultRequest:RequestBase
    {
        public Trip[] Trips { get; set; }
        public string ReturnDate { get; set; }
        [Required(AllowEmptyStrings =false, ErrorMessage ="Max connection must be non empty")]
        public string MaxConnections { get; set; }
        public int MaxNumberOfTrips { get; set; }
        public bool RequestAdditionalFlights { get; set; }
        [Required(AllowEmptyStrings =false, ErrorMessage ="Travel type code must be non empty")]
        public string TravelTypeCode { get; set; }
        public string QualifiedEmergency { get; set; }
        public string EmergencyNature { get; set; }        
        [StringLength(maximumLength: 2, MinimumLength = 2, ErrorMessage = "Search type must be two characters.")]
        [RegularExpression("(OW|RT)|(ow|rt)", ErrorMessage = "Search type either OW or RT")]
        public string SearchType { get; set; }        
        public bool IsPassRiderLoggedIn { get; set; }
        public bool IncludeSSRUMNR { get; set; }
        public string EResSessionId { get; set; }
        public string EResTransactionId { get; set; }
    }

}