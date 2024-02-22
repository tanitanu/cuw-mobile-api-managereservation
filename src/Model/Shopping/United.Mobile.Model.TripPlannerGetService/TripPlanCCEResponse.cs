using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.TripPlannerGetService
{
   public class TripPlanCCEResponse:IPersist
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Shopping.TripPlanCCEResponse";
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
        #endregion IPersist Members

        private List<TripPlanTrip> tripPlanTrips = null;
        private bool isPilot;
        private string tripPlanID;
        private string tripLastUpdate;
        private string tripPlannerCreatorDeviceID;
        private string tripPlannerCreatorMPNumber;
        private string deviceIds;

        public string DeviceIds
        {
            get { return deviceIds; }
            set { deviceIds = value; }
        }

        public string TripPlannerCreatorMPNumber
        {
            get { return tripPlannerCreatorMPNumber; }
            set { tripPlannerCreatorMPNumber = value; }
        }


        public string TripPlannerCreatorDeviceID
        {
            get { return tripPlannerCreatorDeviceID; }
            set { tripPlannerCreatorDeviceID = value; }
        }


        public string TripLastUpdate
        {
            get { return tripLastUpdate; }
            set { tripLastUpdate = value; }
        }


        public bool IsPilot
        {
            get { return isPilot; }
            set { isPilot = value; }
        }

        public List<TripPlanTrip> TripPlanTrips
        {
            get { return tripPlanTrips; }
            set { tripPlanTrips = value; }
        }

        public string TripPlanID
        {
            get { return tripPlanID; }
            set { tripPlanID = value; }
        }
    }
}
