using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class MOBInternationalBilling
    {
        private List<MOBCPBillingCountry> billingAddressProperties;
        public List<MOBCPBillingCountry> BillingAddressProperties
        {
            get
            {
                return this.billingAddressProperties;
            }
            set
            {
                this.billingAddressProperties = value;
            }
        }
    }

}
