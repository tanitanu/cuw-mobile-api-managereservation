using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Shopping.Common.TripPlanner
{
    public class MOBTripPlanShopHelper
    {
        private string tPSessionId = string.Empty;
        private string tripPlanCartId = string.Empty;
        //private string hashPinCode;
        private string tripPlanId = null;
        private MOBSHOPTripPlannerType tripPlannerType = MOBSHOPTripPlannerType.Copilot;
        private MOBSHOPShopRequest mobShopRequest = null;
        private bool isTravelCountChanged;

        public bool IsTravelCountChanged
        {
            get
            {
                return this.isTravelCountChanged;
            }
            set
            {
                this.isTravelCountChanged = value;
            }
        }
        public MOBSHOPShopRequest MobShopRequest
        {
            get
            {
                return this.mobShopRequest;
            }
            set
            {
                this.mobShopRequest = value;
            }
        }



        public MOBSHOPTripPlannerType TripPlannerType
        {
            get
            {
                return this.tripPlannerType;
            }
            set
            {
                this.tripPlannerType = value;
            }
        }

        public string TPSessionId
        {
            get
            {
                return this.tPSessionId;
            }
            set
            {
                this.tPSessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TripPlanCartId
        {
            get
            {
                return this.tripPlanCartId;
            }
            set
            {
                this.tripPlanCartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TripPlanId
        {
            get
            {
                return this.tripPlanId;
            }
            set
            {
                this.tripPlanId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }


    }
}
