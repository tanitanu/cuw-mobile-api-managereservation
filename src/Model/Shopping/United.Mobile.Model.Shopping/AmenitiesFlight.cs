using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class AmenitiesFlight
    {

        public string TripId { get; set; } = string.Empty;
       
        public string FlightId { get; set; } = string.Empty;
      
        public string FlightNumber { get; set; } = string.Empty;
       
        public bool HasWifi { get; set; } 
      
        public bool HasInSeatPower { get; set; } 

        public bool HasDirecTV { get; set; } 

        public bool HasAVOnDemand { get; set; } 

        public bool HasBeverageService { get; set; } 

        public bool HasEconomyLieFlatSeating { get; set; } 

        public bool HasEconomyMeal { get; set; } 

        public bool HasFirstClassMeal { get; set; } 
        
        public bool HasFirstClassLieFlatSeating { get; set; } 

    }
}
