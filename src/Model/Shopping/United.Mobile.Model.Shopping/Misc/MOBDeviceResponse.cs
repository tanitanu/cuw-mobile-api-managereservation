using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class DeviceResponse : MOBResponse
    {
        private string guid = string.Empty;
        private int deviceID;

        public DeviceResponse()
            : base()
        {
        }

        public string GUID
        {
            get
            {
                return this.guid;
            }
            set
            {
                this.guid = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int DeviceID
        {
            get
            {
                return this.deviceID;
            }
            set
            {
                this.deviceID = value;
            }
        }

    }
}
