using System;

namespace United.Mobile.Model.PreOrderMeals
{
    [Serializable]
    public class PreOrderMealCartResponse : MOBResponse
    {
        private string confirmationNumber;

        public string ConfirmationNumber
        {
            get { return confirmationNumber; }
            set { confirmationNumber = value; }
        }

        private string cartId;

        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }

        private string orderId;

        public string OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }

        private bool isSuccess;

        public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }


    }
}
