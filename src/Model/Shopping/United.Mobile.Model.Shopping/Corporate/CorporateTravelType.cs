using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Corporate
{
    [Serializable]
    public class CorporateTravelType
    {

        public List<CorporateTravelTypeItem> CorporateTravelTypes { get; set; } 
        public bool CorporateCustomer { get; set; } 
        public bool CorporateCustomerBEAllowed { get; set; } 

        public MOBCorporateDetails CorporateDetails { get; set; } 

    }
}
