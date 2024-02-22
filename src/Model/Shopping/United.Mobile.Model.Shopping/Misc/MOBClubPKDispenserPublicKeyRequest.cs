using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ClubPKDispenserPublicKeyRequest : MOBRequest
    {
        private string mpNumber;

        public string MPNumber
        {
            get
            {
                return this.mpNumber;
            }
            set
            {
                this.mpNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        
    }
}
