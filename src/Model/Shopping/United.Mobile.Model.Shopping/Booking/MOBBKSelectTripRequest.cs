using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable()]
    public class MOBBKSelectTripRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string tripId = string.Empty;
        private string flightId = string.Empty;
        private string rewardId = string.Empty;

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

        public string TripId
        {
            get
            {
                return this.tripId;
            }
            set
            {
                this.tripId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FlightId
        {
            get
            {
                return this.flightId;
            }
            set
            {
                this.flightId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string RewardId
        {
            get
            {
                return this.rewardId;
            }
            set
            {
                this.rewardId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

    }
}
