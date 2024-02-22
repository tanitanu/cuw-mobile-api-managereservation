using System.Collections.Generic;
using United.Mobile.Model.Common;
using EResAlert = United.Mobile.Model.Internal.Common.EResAlert;

namespace United.Mobile.Model.CompleteBookingContent
{

    public class CompleteBookingResponse:ResponseBase
    {
     
        public Message BookingCompleteMessage { get; set; }
        public string BagRuleLabelText { get; set; }
        public string AddToCalendarLabelText { get; set; }
        public string OffsetCabinUrl { get; set; }
        public string AdultContacts { get; set; }
        public bool IsCompletPaymentFailure { get; set; }        
        public string TotalCost { get; set; }
        public string TotalPriceLabelText { get; set; }
        public bool ShowPaymentDeductedSection { get; set; }
        public string PayrollDeductingMessageContent { get; set; }
        public string CompleteBookingButtonText { get; set; }         
        public string ViewDetailsLabelText { get; set; }        
        public CompleteBookingDisplayCart DisplayCart { get; set; }
        public string EResTransactionId { get; set; }        
        public string BookingSessionId { get; set; }
        public bool IsCheckIn { get; set; }
        public bool ShowBusinessDisplacementCost { get; set; }
        public string BusinessDisplacementCostTextLine1 { get; set; }
        public string BusinessDisplacementCostTextLine2 { get; set; }
        public string BusinessDisplacementCost { get; set; }
        public List<EResAlert>Alerts { get; set; }
    }
   
}
