using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.CancelReservation
{
    public class EResCancelReservationRequest : EResBaseRequest
    {
        public string TravelPlan { get; set; }
        public string SegmentNumber { get; set; } 
        public string PassClass { get; set; } 
        public string Origin { get; set; } 
        public string Destination { get; set; }
        public string ArrivalTime { get; set; } 
        public string DepartureTime { get; set; } 
    }
}
