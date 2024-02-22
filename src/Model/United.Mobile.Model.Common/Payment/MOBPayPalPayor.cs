using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBPayPalPayor
    {
        private string payPallCustomerID;
        private string payPalContactEmailAddress;
        private string payPalContactPhoneNumber;
        private string payPalGivenName;
        private string payPalSurname;
        private string payPalStatus; // EX: Description=VERIFIED
        private MOBAddress payPalBillingAddress;

        public MOBAddress PayPalBillingAddress
        {
            get { return payPalBillingAddress; }
            set { payPalBillingAddress = value; }
        }


        public string PayPalStatus
        {
            get { return payPalStatus; }
            set { payPalStatus = value; }
        }

        [JsonProperty(PropertyName = "payPalSurname")]
        [JsonPropertyName("payPalSurname")]
        public string PayPalSurName
        {
            get { return payPalSurname; }
            set { payPalSurname = value; }
        }


        public string PayPalGivenName
        {
            get { return payPalGivenName; }
            set { payPalGivenName = value; }
        }


        public string PayPalContactEmailAddress
        {
            get { return payPalContactEmailAddress; }
            set { payPalContactEmailAddress = value; }
        }

        [JsonProperty(PropertyName = "payPallCustomerID")]
        [JsonPropertyName("payPallCustomerID")]
        public string PayPalCustomerID
        {
            get { return payPallCustomerID; }
            set { payPallCustomerID = value; }
        }

        public string PayPalContactPhoneNumber
        {
            get { return payPalContactPhoneNumber; }
            set { payPalContactPhoneNumber = value; }
        }
    }
}
