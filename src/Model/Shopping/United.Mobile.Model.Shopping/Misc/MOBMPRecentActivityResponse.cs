using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MPRecentActivityResponse : MOBResponse
    {
        private MPRecentActivity recentActivity;

        public MPRecentActivityResponse()
            : base()
        {
        }

        public MPRecentActivity RecentActivity
        {
            get
            {
                return this.recentActivity;
            }
            set
            {
                this.recentActivity = value;
            }
        }
    }
}
