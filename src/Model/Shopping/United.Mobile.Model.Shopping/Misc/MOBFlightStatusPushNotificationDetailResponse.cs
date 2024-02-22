using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlightStatusPushNotificationDetailResponse : MOBResponse
    {
        private string notificationId = string.Empty;
        private Notification notification;

        public FlightStatusPushNotificationDetailResponse()
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

        public Notification Notification
        {
            get
            {
                return this.notification;
            }
            set
            {
                this.notification = value;
            }
        }
    }
}
