using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class CheckOutResponse : MOBResponse
    {
        private string sessionId;
        private string flow = string.Empty;
        private MOBShoppingCart shoppingCart;
        private string postPurchasePage;
        private string recordLocator = string.Empty;
        private string pnrCreateDate;
        private string lastName = string.Empty;
        private bool enabledSecondaryFormofPayment = false;
        private bool isTPIFailed = false;
        private List<string> seatAssignMessages = new List<string>();
        private bool isUpgradePartialSuccess = false;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        public string Flow
        {
            get { return flow; }
            set { flow = value; }
        }

        public MOBShoppingCart ShoppingCart
        {
            get { return shoppingCart; }
            set { shoppingCart = value; }
        }

        public string PostPurchasePage
        {
            get { return postPurchasePage; }
            set { postPurchasePage = value; }
        }

        public string RecordLocator
        {
            get { return recordLocator; }
            set { recordLocator = value; }
        }
        public string PnrCreateDate
        {
            get { return pnrCreateDate; }
            set { pnrCreateDate = value; }
        }
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public bool EnabledSecondaryFormofPayment
        {
            get { return enabledSecondaryFormofPayment; }
            set { enabledSecondaryFormofPayment = value; }
        }

        public bool IsTPIFailed
        {
            get { return isTPIFailed; }
            set { isTPIFailed = value; }
        }
        public List<string> SeatAssignMessages
        {
            get
            {
                return this.seatAssignMessages;
            }
            set
            {
                this.seatAssignMessages = value;
            }
        }

        public bool IsUpgradePartialSuccess
        { get { return this.isUpgradePartialSuccess; } set { this.isUpgradePartialSuccess = value; } }
    }
}
