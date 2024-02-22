using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class Notification
    {
        string notificationId = string.Empty;
        string deviceId = string.Empty;
        string alert = string.Empty;
        string sound = "default";
        string badge = "1";
        List<MOBKVP> customPayload;
        List<MOBKVP> additionalInfo;

        public string NotificationId
        {
            get { return notificationId; }
            set { notificationId = value; }
        }

        public string DeviceId
        {
            get { return deviceId; }
            set { deviceId = value; }
        }

        public string Alert
        {
            get { return alert; }
            set { alert = value; }
        }

        public string Sound
        {
            get { return sound; }
            set { sound = value; }
        }

        public string Badge
        {
            get { return badge; }
            set { badge = value; }
        }

        public List<MOBKVP> CustomPayload
        {
            get { return customPayload; }
            set { customPayload = value; }
        }

        public List<MOBKVP> AdditionalInfo
        {
            get { return additionalInfo; }
            set { additionalInfo = value; }
        }
    }
}
