using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.ManageRes
{
    [Serializable]
    public class MOBCancelAndRefundReservationResponse : MOBModifyReservationResponse
    {
        private string pnr;
        private string email;
        private string customerServicePhoneNumber;
        private List<MOBRefundOption> selectedRefundOptions;
        private MOBModifyFlowPricingInfo pricing;
        private List<MOBModifyFlowPricingInfo> quotes;
        private List<MOBModifyFlowPricingInfo> pricingItems;
        private List<MOBPNRAdvisory> advisoryInfo;


        public List<MOBRefundOption> SelectedRefundOptions { get { return selectedRefundOptions; } set { selectedRefundOptions = value; } }
        public MOBModifyFlowPricingInfo Pricing { get { return pricing; } set { pricing = value; } }
        public List<MOBModifyFlowPricingInfo> PricingItems { get { return pricingItems; } set { pricingItems = value; } }
        public string Pnr
        {
            get { return pnr; }
            set { pnr = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string CustomerServicePhoneNumber
        {
            get { return customerServicePhoneNumber; }
            set { customerServicePhoneNumber = value; }
        }
        public List<MOBModifyFlowPricingInfo> Quotes
        {
            get { return quotes; }
            set { quotes = value; }
        }

        public List<MOBPNRAdvisory> AdvisoryInfo { get { return this.advisoryInfo; } set { this.advisoryInfo = value; } }

    }
}
