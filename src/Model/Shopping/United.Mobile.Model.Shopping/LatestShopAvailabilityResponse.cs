using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class LatestShopAvailabilityResponse 
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Shopping.LatestShopAvailabilityResponse";
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

        public string SessionId { get; set; }
        public List<string> AvailabilityKeys { get; set; }
        public SerializableDictionary<string, MOBSHOPAvailability> AvailabilityList { get; set; }
        public string CartId { get; set; }
    }
}