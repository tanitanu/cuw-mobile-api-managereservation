using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Shopping;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.TripPlannerGetService
{
    [Serializable]
    public class MOBSHOPSelectTripResponse : MOBResponse
    {
        private MOBSHOPSelectTripRequest request;
        private MOBSHOPAvailability availability;
        public MOBSHOPFareLock productOffer;
        private List<string> disclaimer;
        private bool isTokenAuthenticated;
        private MOBSHOPPinDownRequest pinDownRequest;
        private string cartId = string.Empty;
        private string refreshResultsData = string.Empty;
        private MOBShoppingCart shoppingCart;
        private bool isDefaultPaymentOption = false;
        private List<FormofPaymentOption> eligibleFormofPayments;
        private MOBOnScreenAlert nonUSPOSAlertMessage;
        public MOBOnScreenAlert NonUSPOSAlertMessage
        {
            get { return nonUSPOSAlertMessage; }
            set { nonUSPOSAlertMessage = value; }
        }        

        public MOBSHOPSelectTripRequest Request
        {
            get
            {
                return this.request;
            }
            set
            {
                this.request = value;
            }
        }

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

        public bool IsTokenAuthenticated
        {
            get
            {
                return this.isTokenAuthenticated;
            }
            set
            {
                this.isTokenAuthenticated = value;
            }
        }

        public MOBSHOPPinDownRequest PinDownRequest
        {
            get
            {
                return this.pinDownRequest;
            }
            set
            {
                this.pinDownRequest = value;
            }
        }

        public string CartId
        {
            get { return cartId; }
            set { cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
        public string RefreshResultsData
        {
            get { return refreshResultsData; }
            set { refreshResultsData = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
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

        public MOBSHOPShopRequest ShopRequest { get; set; }
        public bool ShowEditSearchHeaderOnFSRBooking { get; set; }
    }
}
