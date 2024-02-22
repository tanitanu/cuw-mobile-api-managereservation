using System.Collections.Generic;

namespace United.Mobile.Model.Internal.CompleteBooking
{
    public class PaymentFailureMsg
    {
        public List<string> TravelPlan { get; set; }
        public string Message { get; set; }
    }


}