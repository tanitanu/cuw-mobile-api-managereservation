using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBEmpUASubscriptions
    {
        public MOBEmpUASubscriptions()
            : base()
        {
        }

        private string mpAccountNumber;
        private List<MOBEmpUASubscription> subscriptionTypes;

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

        public List<MOBEmpUASubscription> SubscriptionTypes
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
