using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.TripPlannerGetService
{
    [Serializable]
    public class MOBSHOPPinDownRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string mileagePlusAccountNumber = string.Empty;
        private int premierStatusLevel;
        private MOBSHOPPinDownAvailability availability;

        public string SessionId
        {
            get
            {
                return sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MileagePlusAccountNumber
        {
            get
            {
                return mileagePlusAccountNumber;
            }
            set
            {
                this.mileagePlusAccountNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public MOBSHOPPinDownAvailability Availability
        {
            get
            {
                return this.availability;
            }
            set
            {
                this.availability = value;
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
    }
}
