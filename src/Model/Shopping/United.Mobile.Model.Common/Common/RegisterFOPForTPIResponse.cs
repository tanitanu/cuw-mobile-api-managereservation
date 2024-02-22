using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class RegisterFOPForTPIResponse : MOBResponse
    {
        public double Amount { get; set; }

        public string DisplayAmount { get; set; } = string.Empty;

        public string ConfirmationResponseMessage { get; set; } = string.Empty;

        public string ConfirmationResponseEmailMessage { get; set; } = string.Empty;

        public string ConfirmationResponseEmail { get; set; } = string.Empty;

        public List<string> ConfirmationResponseDetailMessage { get; set; }

        public string RecordLocator { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
    }
}
