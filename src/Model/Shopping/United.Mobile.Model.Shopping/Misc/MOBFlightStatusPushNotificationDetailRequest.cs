using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlightStatusPushNotificationDetailRequest : MOBRequest
    {
        private string notificationId = string.Empty;

        public FlightStatusPushNotificationDetailRequest()
            : base()
        {
        }

        public string NotificationId
        {
            get
            {
                return this.notificationId;
            }
            set
            {
                this.notificationId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

    }
}
