using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.Internal.CompleteBooking;

namespace United.Mobile.Model.Internal.Booking
{
    public class EmpBookingTripsResponse:EResBaseResponse
    {
        public DisplayCart DisplayCart { get; set; }
        public string DuplicateErrorMsg { get; set; }
        public object FormOfPayment { get; set; }
        public bool IsCompletPaymentFailure { get; set; }
        public bool IsUMNRTravelling { get; set; }
        public string OffsetCabinUrl { get; set; }
        public PaymentFailureMsg PaymentFailureMsg { get; set; }
        public object PRNs { get; set; }
        public object AdultContacts { get; set; }
        public decimal TotalCost { get; set; }

    }
}
