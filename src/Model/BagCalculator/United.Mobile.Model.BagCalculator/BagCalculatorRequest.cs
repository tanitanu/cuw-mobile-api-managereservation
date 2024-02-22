using System;

namespace United.Mobile.Model.BagCalculator
{
    public class BagCalculatorRequest
    {
        public string SessionId { get; set; }
        public string accessCode { get; set; }
        public string transactionId { get; set; }
        public string languageCode { get; set; }
        public string appVersion { get; set; }
        public int applicationId { get; set; }
        public string recordLocator { get; set; }
        public string lastname { get; set; }
        public string departureCode { get; set; }
        public string arrivalCode { get; set; }
        public string departDate { get; set; }
        public string classOfService { get; set; }
        public string loyaltyLevel { get; set; }
        public string ticketingDate { get; set; }
        public string marketingCarrierCode { get; set; }

    }
}
