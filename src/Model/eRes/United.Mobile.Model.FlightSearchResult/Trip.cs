using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.FlightSearchResult
{
    public  class Trip
   {
        public int TripID { get; set; }
        public string DepartDate { get; set; }
        [Required]
        [StringLength(maximumLength:3,MinimumLength =3, ErrorMessage ="Origin must be three characters with valid airport code")]
        public string Origin { get; set; }
        [Required]
        [StringLength(maximumLength: 3, MinimumLength = 3, ErrorMessage = "Destination must be three characters with valid airport code")]
        public string Destination { get; set; }
        public string Time { get; set; }
        //[Required]
        //[MinLength(length:1,ErrorMessage ="At least a employee or dependant(s) id should be passed")]
        public List<string> DependantID { get; set; }
        public int OriginMiles { get; set; }
        public int DestinationMiles { get; set; }
    }
}
