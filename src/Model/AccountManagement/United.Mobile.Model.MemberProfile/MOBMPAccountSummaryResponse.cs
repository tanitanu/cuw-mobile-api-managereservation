using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBMPAccountSummaryResponse : MOBResponse
    {
        private MOBMPAccountSummary opAccountSummary;
        private MOBProfile profile;
        private bool isUASubscriptionsAvailable;
        private MOBUASubscriptions uaSubscriptions;

        public MOBMPAccountSummaryResponse()
            : base()
        {
        }


        public MOBMPAccountSummary OPAccountSummary
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

        public MOBProfile Profile
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

        public bool ISUASubscriptionsAvailable
        {
            get
            {
                return this.isUASubscriptionsAvailable;
            }
            set
            {
                this.isUASubscriptionsAvailable = value;
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
