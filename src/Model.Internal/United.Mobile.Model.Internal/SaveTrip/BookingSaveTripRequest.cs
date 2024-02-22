using System;
using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.Internal.SaveTrip
{
    public class BookingSaveTripRequest
    {
        [Required]
        public string SessionId { get; set; }
        [Required]
        public string SegmentIndex { get; set; }
        [Required]
        public int TripIndex { get; set; }
        [Required]
        public bool RequestAdditionalFlights { get; set; }
        [Required]
        public int MaxNumberOfTrips { get; set; }
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public bool IncludeSSRUMNR { get; set; }
        [Required]
        public Int64 BookingSessionDetailId { get; set; }
    }


}
