using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.TripPlannerGetService
{
    [Serializable()]
    public class MOBTripPlanSummaryRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string tripPlanId = string.Empty;
        private string mpNumber;
        private List<MOBTripPlanSelectTrip> listTripPlanSelectTrip;
        private List<MOBTravelerType> travelerTypes = null;
        private bool isTravelCountChanged;
        private string travelType;
        //private string deleteTripCartId;
        private string hashPinCode;

        public string HashPinCode
        {
            get
            {
                return hashPinCode;
            }
            set
            {
                this.hashPinCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TravelType
        {
            get { return travelType; }
            set { travelType = value; }
        }
        public bool IsTravelCountChanged
        {
            get { return isTravelCountChanged; }
            set { isTravelCountChanged = value; }
        }
        public string TripPlanId
        {
            get { return tripPlanId; }
            set { tripPlanId = value; }
        }
        public string MpNumber
        {
            get { return mpNumber; }
            set { mpNumber = value; }
        }

        public List<MOBTripPlanSelectTrip> ListTripPlanSelectTrip
        {
            set
            {
                listTripPlanSelectTrip = value;
            }
            get
            {
                return listTripPlanSelectTrip;
            }
        }

        public string SessionId
        {
            set { sessionId = value; }
            get { return sessionId; }
        }

        public List<MOBTravelerType> TravelerTypes
        {
            get { return travelerTypes; }
            set { travelerTypes = value; }
        }

        public string deleteTripCartId
        {
            get;
            set;
        }
    }
}
