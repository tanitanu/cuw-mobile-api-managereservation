using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.BagCalculator
{
    
   public class DOTCheckedBagCalculatorRequest: MOBRequest
    {
        public string DepartureCode { get; set; }
        public string ArrivalCode { get; set; }
        public string DepartDate { get; set; } 
        public string ClassOfService { get; set; }
        public string LoyaltyLevel { get; set; }
        public string TicketingDate { get; set; }
        public string MarketingCarrierCode { get; set; }
        public string RecordLocator { get; set; }
        public string LastName { get; set; }
        public string SessionId { get; set; }
        public string Flow { get; set; } = string.Empty;
        public string CartId { get; set; }
        public int PremierStatusLevel { get; set; } = -1;
    }
}
