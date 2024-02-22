using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlightStatusPushNotificationUnsubscribeResponse : MOBResponse
    {
        private string id = string.Empty;
        private string succeed = string.Empty;

        public FlightStatusPushNotificationUnsubscribeResponse()
            : base()
        {
        }

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }



        public string Succeed
        {
            get
            {
                return this.succeed;
            }
            set
            {
                this.succeed = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
