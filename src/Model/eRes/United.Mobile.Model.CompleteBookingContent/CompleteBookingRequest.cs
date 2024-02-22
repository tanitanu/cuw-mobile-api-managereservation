using System.ComponentModel.DataAnnotations;
using United.Mobile.Model.Payment;

namespace United.Mobile.Model.CompleteBookingContent
{
    public class CompleteBookingRequest
    {
        public string Email { get; set; }
        [Required]
        public string SessionId { get; set; }
        [Required]
        public string BookingTravelType { get; set; }
        [Required]
        public string SearchType { get; set; }
        [Required]
        public bool IsPrepaidPayment { get; set; }
        [Required]
        public bool IsAgentToolLogOn { get; set; }
        [Required]
        public bool IsPassRiderLoggedIn { get; set; }
        public string Amount { get; set; }
           
        public CreditCard CreditCard { get; set; }
        public string EncryptedCC { get; set; }
        public bool IsOAEPPaddingCatalogEnabled { get; set; }
    }
}
