using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using United.Mobile.Model.Common.Shopping;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    [XmlRoot("MOBSHOPSelectTripResponse")]
    public class SelectTripResponse : MOBResponse
    {
        public SelectTripRequest Request { get; set; }

        public MOBSHOPAvailability Availability { get; set; }

        public FareLock productOffer { get; set; }

        public List<string> Disclaimer { get; set; }

        public bool IsTokenAuthenticated { get; set; }

        public PinDownRequest PinDownRequest { get; set; }

        public string CartId { get; set; } = string.Empty;

        public string RefreshResultsData { get; set; } = string.Empty;

        public MOBShoppingCart ShoppingCart { get; set; }

        public bool IsDefaultPaymentOption { get; set; }

        public List<FormofPaymentOption> EligibleFormofPayments { get; set; }

        private MOBOnScreenAlert nonUSPOSAlertMessage;
        public MOBOnScreenAlert NonUSPOSAlertMessage
        {
            get { return nonUSPOSAlertMessage; }
            set { nonUSPOSAlertMessage = value; }
        }

        public MOBSHOPShopRequest ShopRequest { get; set; }
        public bool ShowEditSearchHeaderOnFSRBooking { get; set; }

        public SelectTripResponse()
        {
            Disclaimer = new List<string>();
        }
    }
}
