using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPSignIn
{
    [Serializable]
    public class MOBRegisterFOPForTPIResponse : MOBResponse
    {
        private double amount;
        private string displayAmount;
        private string confirmationResponseMessage = string.Empty;
        private string confirmationResponseEmailMessage = string.Empty;
        private string confirmationResponseEmail = string.Empty;
        private List<string> confirmationResponseDetailMessage;
        private string recordLocator = string.Empty;
        private string lastName = string.Empty;

        public double Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                this.amount = value;
            }
        }
        public string DisplayAmount
        {
            get
            {
                return this.displayAmount;
            }
            set
            {
                this.displayAmount = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string ConfirmationResponseMessage
        {
            get
            {
                return this.confirmationResponseMessage;
            }
            set
            {
                this.confirmationResponseMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ConfirmationResponseEmailMessage
        {
            get
            {
                return this.confirmationResponseEmailMessage;
            }
            set
            {
                this.confirmationResponseEmailMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string ConfirmationResponseEmail
        {
            get
            {
                return this.confirmationResponseEmail;
            }
            set
            {
                this.confirmationResponseEmail = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public List<string> ConfirmationResponseDetailMessage
        {
            get { return confirmationResponseDetailMessage; }
            set { confirmationResponseDetailMessage = value; }
        }
        public string RecordLocator
        {
            get
            {
                return this.recordLocator;
            }
            set
            {
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
