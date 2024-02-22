using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpRequest : MOBRequest
    {
        public MOBEmpRequest()
            : base()
        {
        }

        private string tokenId;

        public string TokenId
        {
            get { return this.tokenId; }
            set { this.tokenId = value; }
        }


        private string sessionId = string.Empty;
        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
