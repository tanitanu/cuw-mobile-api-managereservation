using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ClubPKDispenserPublicKeyResponse :MOBResponse
    {
        private string mpNumber;
        private string pkDispenserPublicKey;
        private string sessionId;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        public string PkDispenserPublicKey
        {
            get { return pkDispenserPublicKey; }
            set { pkDispenserPublicKey = value; }
        }
        
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
