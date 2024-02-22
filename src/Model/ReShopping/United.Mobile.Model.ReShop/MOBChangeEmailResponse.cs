using System;
using System.Collections.Generic;
using United.Mobile.Model.Common.Shopping;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.FormofPayment;

namespace United.Mobile.Model.ReShop
{
    [Serializable]
    public class MOBChangeEmailResponse : MOBResponse
    {
        private string sessionId = string.Empty;
        private MOBSHOPReservation reservation;
        private string flow = string.Empty;
        private MOBShoppingCart shoppingCart = new MOBShoppingCart();
        private bool isDefaultPaymentOption = false;
        private List<FormofPaymentOption> eligibleFormofPayments;

        public MOBSHOPReservation Reservation
        {
            get
            {
                return this.reservation;
            }
            set
            {
                this.reservation = value;
            }
        }
        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
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
