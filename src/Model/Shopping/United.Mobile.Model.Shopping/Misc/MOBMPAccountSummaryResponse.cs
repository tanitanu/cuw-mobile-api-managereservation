using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MPAccountSummaryResponse : MOBResponse
    {
        private MPAccountSummary opAccountSummary;
        private MOBSHOPProfile profile;
        public bool isUASubscriptionsAvailable { get; set; }

        private MOBUASubscriptions uaSubscriptions;

        public MPAccountSummaryResponse()
            : base()
        {
        }


        public MPAccountSummary OPAccountSummary
        {
            get
            {
                return this.opAccountSummary;
            }
            set
            {
                this.opAccountSummary = value;
            }
        }

        public MOBSHOPProfile Profile
        {
            get
            {
                return this.profile;
            }
            set
            {
                this.profile = value;
            }
        }

        public MOBUASubscriptions UASubscriptions
        {
            get
            {
                return this.uaSubscriptions;
            }
            set
            {
                this.uaSubscriptions = value;
            }
        }
    }
}
