using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MPRecentActivity
    {
        private string mileagePlusNumber = string.Empty;
        private List<MPActivity> airlineActivities;
        private List<MPActivity> nonAirlineActivities;
        private List<MPActivity> rewardAirlineActivities;
        private List<MPActivity> feqMilesActivities;
        private List<ActivityItem> activityItemList;
        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public List<MPActivity> AirlineActivities
        {
            get
            {
                return this.airlineActivities;
            }
            set
            {
                this.airlineActivities = value;
            }
        }

        public List<MPActivity> NonAirlineActivities
        {
            get
            {
                return this.nonAirlineActivities;
            }
            set
            {
                this.nonAirlineActivities = value;
            }
        }

        public List<MPActivity> RewardAirlineActivities
        {
            get
            {
                return this.rewardAirlineActivities;
            }
            set
            {
                this.rewardAirlineActivities = value;
            }
        }

        public List<MPActivity> FEQMilesActivities
        {
            get
            {
                return this.feqMilesActivities;
            }
            set
            {
                this.feqMilesActivities = value;
            }
        }
        public List<ActivityItem> ActivityItemList
        {
            get { return activityItemList; }
            set { activityItemList = value; }
        }
    }
}
