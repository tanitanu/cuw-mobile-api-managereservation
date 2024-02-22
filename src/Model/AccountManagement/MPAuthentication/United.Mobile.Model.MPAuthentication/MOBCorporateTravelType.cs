using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable]
    public class MOBCorporateTravelType
    {


        private List<MOBCorporateTravelTypeItem> corporateTravelTypes;
        public List<MOBCorporateTravelTypeItem> CorporateTravelTypes
        {
            get
            {
                return corporateTravelTypes;
            }
            set
            {
                corporateTravelTypes = value;
            }
        }

        private bool corporateCustomer;
        public bool CorporateCustomer
        {
            get
            {
                return corporateCustomer;
            }
            set { corporateCustomer = value; }
        }

        private bool corporateCustomerBEAllowed;
        public bool CorporateCustomerBEAllowed
        {
            get
            {
                return corporateCustomerBEAllowed;
            }
            set { corporateCustomerBEAllowed = value; }
        }

        private MOBCorporateDetails corporateDetails;

        public MOBCorporateDetails CorporateDetails
        {
            get { return corporateDetails; }
            set { corporateDetails = value; }
        }

    }
}
