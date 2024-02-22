using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBUASubscriptions
    {
        public MOBUASubscriptions()
            : base()
        {
        }

        private string mpAccountNumber;
        private List<MOBUASubscription> subscriptionTypes;

        public string MPAccountNumber
        {
            get
            {
                return this.mpAccountNumber;
            }
            set
            {
                this.mpAccountNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBUASubscription> SubscriptionTypes
        {
            get
            {
                return this.subscriptionTypes;
            }
            set
            {
                this.subscriptionTypes = value;
            }
        }
    }
}
