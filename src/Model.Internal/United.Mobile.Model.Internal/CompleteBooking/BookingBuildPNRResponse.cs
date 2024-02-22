using System.Collections.Generic;
using System.Collections.ObjectModel;
using United.Mobile.Model.Internal.Booking;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.CompleteBooking
{
    public class BookingBuildPNRResponse : EResBaseResponse
    {
       public DisplayCart DisplayCart { get; set; } = new DisplayCart();
       public List<BookingPNR> PNRs { get; set; } = new List<BookingPNR>();
       public decimal TotalCost { get; set; } = 0.0M;
       public string OffsetCabinUrl { get; set; } = string.Empty;
       public Collection<Payment> FormOfPayment { get; set; } = null;
       public PaymentFailureMsg PaymentFailureMsg { get; set; } = null;
       public bool IsUMNRTravelling { get; set; } = false;
       public List<UnaccompaniedMinorAdultContactPerson> AdultContacts { get; set; } = null;
       public EResAlert DuplicateErrorMsg { get; set; } = null;
       public bool IsCompletPaymentFailure { get; set; } = false;
       public bool IsValidCreditCard { get; set; }
      

    }
}
