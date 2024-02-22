using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class GetAPNSTokenRequest : MOBRequest
    {
        private string deviceId = string.Empty;

        public GetAPNSTokenRequest()
            : base()
        {
        }

        public string DeviceId
        {
            get
            {
                return deviceId;
            }
            set
            {
                this.deviceId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
