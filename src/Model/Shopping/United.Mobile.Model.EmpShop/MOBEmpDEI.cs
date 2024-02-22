using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpDEI
    {
    
        private List<MOBEmpCabinIndicator> cabinIndicators;
        private string serviceClasses;
        private string marketingCarrierName;
        private string operatingCarrierName;

        public List<MOBEmpCabinIndicator> CabinIndicators
        {
            get { return cabinIndicators; }
            set { cabinIndicators = value; }
        }

        public string ServiceClasses
        {
            get { return serviceClasses; }
            set { serviceClasses = value; }
        }

        public string MarketingCarrierName
        {
            get { return marketingCarrierName; }
            set { marketingCarrierName = value; }
        }

        public string OperatingCarrierName
        {
            get { return operatingCarrierName; }
            set { operatingCarrierName = value; }
        }
        
    }
}
