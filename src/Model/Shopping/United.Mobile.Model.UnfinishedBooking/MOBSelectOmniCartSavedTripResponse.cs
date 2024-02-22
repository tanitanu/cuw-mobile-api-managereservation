using System;
using System.Collections.Generic;
using United.Mobile.Model.Common.Shopping;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.UnfinishedBooking
{
    [Serializable]
    public class MOBSelectOmniCartSavedTripResponse : MOBResponse
    {
        private MOBSHOPAvailability availability;
        private List<string> disclaimer;
        private string cartId = string.Empty;
        private MOBShoppingCart shoppingCart;
        private bool isDefaultPaymentOption = false;
        private List<FormofPaymentOption> eligibleFormofPayments;

        public MOBSHOPAvailability Availability
        {
            get
            {
                return this.availability;
            }
            set
            {
                this.availability = value;
            }
        }

        public List<string> Disclaimer
        {
            get
            {
                return this.disclaimer;
            }
            set
            {
                this.disclaimer = value;
            }
        }



        public string CartId
        {
            get { return cartId; }
            set { cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public MOBShoppingCart ShoppingCart
        {
            get { return shoppingCart; }
            set { shoppingCart = value; }
        }
        public bool IsDefaultPaymentOption
        {
            get
            {
                return this.isDefaultPaymentOption;
            }
            set
            {
                this.isDefaultPaymentOption = value;
            }
        }
        public List<FormofPaymentOption> EligibleFormofPayments
        {
            get { return eligibleFormofPayments; }
            set { eligibleFormofPayments = value; }
        }
    }
}
