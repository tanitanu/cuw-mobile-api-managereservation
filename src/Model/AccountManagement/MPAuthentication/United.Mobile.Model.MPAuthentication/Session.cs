using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class Session 
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Shopping.Session";
        private int shopHasNonStop = -1;
        private int selectTripHasNonStop = -1;

        public string ObjectName
        {
            get
            {
                return this.objectName;
            }
            set
            {
                this.objectName = value;
            }
        }

        #endregion

        public string Token { get; set; }
        public bool IsTokenAuthenticated { get; set; }
        public DateTime TokenExpireDateTime { get; set; }
        public double TokenExpirationValueInSeconds { get; set; }
        public bool IsTokenExpired { get; set; }
        public string SessionId { get; set; }
        public string CartId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastSavedTime { get; set; }
        public string LastSelectTripKey { get; set; }
        public int CustomerID { get; set; }
        public string MileagPlusNumber { get; set; }
        public bool SupressLMXForAppID { get; set; }
        public string EmployeeId { get; set; }
        public string DeviceID { get; set; }
        public int AppID { get; set; }
        public string VersionID { get; set; }
        public bool IsMeta { get; set; }
        public int ShopSearchTripCount { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool IsBEFareDisplayAtFSR { get; set; }
        public bool IsReshopChange { get; set; }

        public bool IsAward { get; set; }

        public bool IsExpertModeEnabled { get; set; }

        public bool IsYoungAdult { get; set; }

        public bool IsFSRRedesign { get; set; }


        /// <summary>
        /// -1 : this value has not been set
        /// 0 : this value has not been set and FSR1 has been verified that it doesn't have NonStop 
        /// 1: this value has not been set and FSR1 has been verified that it has NonStop
        /// </summary>
        public int ShopHasNonStop
        {
            get { return shopHasNonStop; }
            set { shopHasNonStop = value; }
        }

        /// <summary>
        /// -1 : this value has not been set
        /// 0 : this value has not been set and FSR2 has been verified that it doesn't have NonStop 
        /// 1: this value has not been set and FSR2 has been verified that it has NonStop
        /// </summary>
        public int SelectTripHasNonStop
        {
            get { return selectTripHasNonStop; }
            set { selectTripHasNonStop = value; }
        }

        public bool IsCorporateBooking { get; set; }

        public bool IsArrangerBooking { get; set; }

        public string Flow { get; set; }
        private string travelType;

        public string TravelType
        {
            get { return travelType; }
            set { travelType = value; }
        }

    }
}
