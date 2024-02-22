using System;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBSHOPInflightContactlessPaymentEligibility
    {
        private bool isEligibleInflightCLPayment;
        private string optTitle;
        private string optMessage;
        private bool isCCSelectedForContactless;

        public bool IsCCSelectedForContactless
        {
            get { return isCCSelectedForContactless; }
            set { isCCSelectedForContactless = value; }
        }

        public MOBSHOPInflightContactlessPaymentEligibility() { }
        public MOBSHOPInflightContactlessPaymentEligibility(bool isEligible, string title, string msg)
        {
            this.isEligibleInflightCLPayment = isEligible;
            this.optTitle = title;
            this.optMessage = msg;
        }

        public bool IsEligibleInflightCLPayment
        {
            get { return isEligibleInflightCLPayment; }
            set { isEligibleInflightCLPayment = value; }
        }

        public string OptTitle
        {
            get { return optTitle; }
            set { optTitle = value; }
        }

        public string OptMessage
        {
            get { return optMessage; }
            set { optMessage = value; }
        }
    }
}
