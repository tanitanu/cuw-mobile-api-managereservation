using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.FlightSearchResult;
using United.Mobile.Model.Internal.Booking;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.BookingTrips
{
    public class BookingTripResponse: ResponseBase
    {
        public string DuplicateBookingMessage { get; set; }
        public string PaymentFailureMessage { get; set; }
        public string OffsetCabinUrl { get; set; }
        public string AdultContacts { get; set; }
        public bool IsCompletPaymentFailure { get; set; }
        public object PNRs { get; set; }
        public string TotalCost { get; set; }
        public string TotalPriceLabelText { get; set; } 
        public string PayrollDeductingMessageContent { get; set; }
        public string BookingItineraryButtonText { get; set; }
        public string EmailAddressLabelText { get; set; } 
        public string ViewDetailsLabelText { get; set; }
        public string PaymentMethodSectionHeaderText { get; set; } 
        public DisplayCart DisplayCart { get; set; }
        public string EResTransactionId { get; set; }
        public string SessionId { get; set; }
        public string BookingSessionId { get; set; }
        public bool ShowBusinessDisplacementCost { get; set; }
        public string BusinessDisplacementCostTextLine1 { get; set; }
        public string BusinessDisplacementCostTextLine2 { get; set; }
        public string BusinessDisplacementCost { get; set; }       
        public List<EResAlert> Alerts { get; set; }

    }    
    
    public class DisplayCart
    {
        public PriceBreakdownDetails PriceBreakdownDetails { get; set; }
        public List<SaveBookingTrip> BookingTrips { get; set; }
        public List<PassengerDetail> BookingPassengers { get; set; }
        public CreditCardInfo CreditCardInfo { get; set; }
        public string EmailForEpassReceipt { get; set; }
        public EResAlert ImputedBillingAlert { get; set; }
        public bool IsCreditCardAvailable { get; set; }
        public bool IsPayrollDeductAvailable { get; set; }
        public List<EResAlert> ToolTips { get; set; }
        public string TotalCost { get; set; }
        public string TotalDisplacementCost { get; set; }
        public decimal TotalImputedIncomeTaxCost { get; set; }
        public string PublicHashKey { get; set; }
        public string PayrollDeductLabelText { get; set; }
        public string PrePayLabelText { get; set; } 
        public bool IsPaymentEnable { get; set; }
        public bool ShowPriceBreakdown { get; set; }
        public string BookingTripTermsAndConditionText { get; set; }
        public string Kid { get; set; }
        public string Exp { get; set; }
    }
  
    public class PriceBreakdownDetails
    {
        public string PriceBreakdownSectionHeader { get; set; } 
        public string TotalPriceLabelText { get; set; } 
        public List<PassengerPrice> PassengerPrices { get; set; }
        public string TotalCost { get; set; }
        //public decimal TotalCostRaw { get; set; }
    }
    public class PassengerPrice
    {
        public string PassengerName { get; set; }
        public string BaseFareText { get; set; } 
        public string BaseFare { get; set; }
        public List<TypeOption> Taxes { get; set; }
    }

    public class PassengerDetail
    {
        public string PassengerName { get; set; }
        public List<TypeOption> KeyValues { get; set; }
        public string AlertMessage { get; set; }
        public string LastName { get; set; }
        public List<SpecialService> SpecialService { get; set; }

    }
}
