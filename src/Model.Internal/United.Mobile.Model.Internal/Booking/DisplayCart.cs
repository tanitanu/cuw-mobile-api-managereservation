using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.Internal.EmployeeProfile;

namespace United.Mobile.Model.Internal.Booking
{
    public class DisplayCart
    {
        public EmployeeAddress AddressVerification { get; set; }
        public List<BookingPassenger> BookingPassengers{ get; set; }
        public List<BookingTrip> BookingTrips { get; set; }
        public CreditCardInfo CreditCardInfo { get; set; }
        public EResAlert CrewAlert { get; set; }
        public List<EResAlert> DestinationAlert { get; set; }
        public string EmailForEpassReceipt { get; set; }
        public EResAlert ImputedBillingAlert { get; set; }
        public bool IsCreditCardAvailable { get; set; }
        public bool IsPayrollDeductAvailable { get; set; }
        public List<EResAlert> ToolTip { get; set; }
        public string TotalCost { get; set; }
        public string TotalDisplacementCost { get; set; }
        public decimal TotalImputedIncomeTaxCost { get; set; }
        public DisplayCart()
        {
            BookingPassengers = new List<BookingPassenger>();
            BookingTrips = new List<BookingTrip>();
            DestinationAlert = new List<EResAlert>();
            ToolTip = new List<EResAlert>();
        }
    }
}
