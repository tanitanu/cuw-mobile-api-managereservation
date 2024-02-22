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
    public class MOBSHOPTripPlanRequest:MOBRequest
    {
        private string sessionId = string.Empty;
        private string cartId = string.Empty;
        private string mileagePlusAccountNumber = string.Empty;
        private int premierStatusLevel;
        //private string hashPinCode;
        private List<MOBTravelerType> travelerTypes = null;
        private string tripPlanId = null;
        private string tripPlannerType = null;
        private string countryCode = null;
        private string travelType = string.Empty;
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

        public string CountryCode
        {
            get { return countryCode; }
            set { countryCode = value; }
        }
        
        //private bool isTravelCountChanged;


        //public bool IsTravelCountChanged
        //{
        //    get
        //    {
        //        return this.isTravelCountChanged;
        //    }
        //    set
        //    {
        //        this.isTravelCountChanged = value;
        //    }
        //}

        public string TripPlannerType
        {
            get
            {
                return string.IsNullOrEmpty(this.tripPlannerType) ? MOBSHOPTripPlannerType.Pilot.ToString() : this.tripPlannerType; 
            }
            set
            {
                this.tripPlannerType = value;
            }
        }

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CartId
        {
            get
            {
                return this.cartId;
            }
            set
            {
                this.cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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


        public string MileagePlusAccountNumber
        {
            get
            {
                return this.mileagePlusAccountNumber;
            }
            set
            {
                this.mileagePlusAccountNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public int PremierStatusLevel
        {
            get
            {
                return this.premierStatusLevel;
            }
            set
            {
                this.premierStatusLevel = value;
            }
        }

        //public string HashPinCode
        //{
        //    get { return hashPinCode; }
        //    set { hashPinCode = value; }
        //}

        public List<MOBTravelerType> TravelerTypes
        {
            get { return travelerTypes; }
            set { travelerTypes = value; }
        }

    }

    public enum MOBSHOPTripPlannerType
    {
        Pilot,
        Copilot
    }
}
