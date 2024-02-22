using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping.Pcu
{
    [Serializable()]
    public class PremiumCabinUpgradeRequest : MOBRequest
    {
        private string sessionId;
        private List<string> selectedSegmentIds;
        private FormOfPayment formOfPayment;
        private MOBAddress billingAddress;
        private MOBCPPhone phone;
        private string emailAddress;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        public List<string> SelectedSegmentIds
        {
            get { return selectedSegmentIds; }
            set { selectedSegmentIds = value; }   
        }
        
        public FormOfPayment FormOfPayment
        {
            get { return formOfPayment; }
            set { formOfPayment = value; }
        }

        public MOBAddress BillingAddress
        {
            get { return billingAddress; }
            set { billingAddress = value; }
        }

        public MOBCPPhone Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }
    }
}
