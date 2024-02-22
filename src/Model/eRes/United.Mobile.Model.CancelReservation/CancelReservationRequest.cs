using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.CancelReservation
{
    public class CancelReservationRequest 
    {
        [Required]
        public string TravelPlan { get; set; }  
        [Required]
        public string PassClass { get; set; }
        [Required]
        public string SessionId { get; set; }       
        [Required]
        public string LastName { get; set; }
        public bool ShowPSLCancel { get; set; }
        public List<string> ConnectedTravelPlan { get; set; } 
    }
}
