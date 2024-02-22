using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.Internal.CompleteBooking
{
    public class CompleteBookingRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string BookingSessionId { get; set; }
        [Required]
        public string TransactionID { get; set; }
        [Required]
        public string BookingTravelType { get; set; }
        [Required]
        public string SearchType { get; set; }
        [Required ]
        public bool IsPrepaidPayment { get; set; }
        [Required]
        public bool isAgentToolLogOn { get; set; }
        [Required]
        public bool IsPassRiderLoggedIn { get; set; }

    }
}
