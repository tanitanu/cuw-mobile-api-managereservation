using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping.Pcu;
using United.Mobile.Model.Shopping.Shopping;

namespace United.Mobile.Model.Shopping.Pcu
{
    [Serializable()]
    public class PremiumCabinUpgradeResponse : MOBResponse
    {
        private PremiumCabinUpgradeRequest request;
        private PcuPurchaseConfirmation purchaseConfirmation;
        
        public PremiumCabinUpgradeRequest Request
        {
            get { return request; }
            set { request = value; }
        }

        public PcuPurchaseConfirmation PurchaseConfirmation
        {
            get { return purchaseConfirmation; }
            set { purchaseConfirmation = value; }
        }

        private MOBSHOPResponseStatusItem responseStatusItem;

        public MOBSHOPResponseStatusItem ResponseStatusItem
        {
            get { return responseStatusItem; }
            set { responseStatusItem = value; }
        }
    }
}
