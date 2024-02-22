using System;

namespace United.Mobile.Model.MPRewards
{
    [Serializable]
    public class MOBPBCustomer
    {
        private string custId = string.Empty; // custId from Merch response 
        private string tagId = string.Empty;  // 1-1, first 1 represents for segment ID, second 1 represents for customer ID
        private string custName = string.Empty; // cust name
        private double fee; // from Merch
        private bool alreadyPurchased = false;
        private bool selected = false; // client side usage 
        private string message = string.Empty; // segment ineligible reason 
        private string formattedFee = string.Empty; // formatted amount with dollar sign. Will be rounded in offer tile and 2 decimal in payment page

        public string CustId
        {
            get { return custId; }
            set { custId = value; }
        }
        public string TagId
        {
            get { return tagId; }
            set { tagId = value; }
        }

        public string CustName
        {
            get { return custName; }
            set { custName = value; }
        }

        public double Fee
        {
            get { return fee; }
            set { fee = value; }
        }

        public bool AlreadyPurchased
        {
            get { return alreadyPurchased; }
            set { alreadyPurchased = value; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public string FormattedFee
        {
            get { return formattedFee; }
            set { formattedFee = value; }
        }
    }
}
