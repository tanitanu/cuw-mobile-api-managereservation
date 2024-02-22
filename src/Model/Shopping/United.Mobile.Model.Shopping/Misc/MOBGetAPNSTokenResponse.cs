using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class GetAPNSTokenResponse : MOBResponse
    {
        private GetAPNSTokenRequest request;
        private string apnsToken = string.Empty;

        public GetAPNSTokenResponse()
            : base()
        {
        }


        public GetAPNSTokenRequest Request
        {
            get
            {
                return this.request;
            }
            set
            {
                this.request = value;
            }
        }

        public string APNSToken
        {
            get
            {
                return apnsToken;
            }
            set
            {
                this.apnsToken = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
