using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class CustomerProfileRequest:CPProfileRequest
    {  
        private string transactionPath = string.Empty;
        private string redirectViewName = string.Empty;
        private bool isRequirePKDispenserPublicKey;

        public bool IsRequirePKDispenserPublicKey
        {
            get { return isRequirePKDispenserPublicKey; }
            set { isRequirePKDispenserPublicKey = value; }
        }
        public string RedirectViewName
        {
            get { return redirectViewName; }
            set { redirectViewName = value; }
        }
        public string TransactionPath
        {
            get { return transactionPath; }
            set { transactionPath = value; }
        }
    }
}
