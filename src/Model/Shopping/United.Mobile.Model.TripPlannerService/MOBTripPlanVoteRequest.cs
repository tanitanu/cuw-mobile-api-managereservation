using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.TripPlannerService
{
    [Serializable()]
    public class MOBTripPlanVoteRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string firstFlightDepartDate;
        private string shoppingCartId;
        private string tripPlanId;
        private string mileagePlusId;
        private string voteId;
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

        public string VoteId
        {
            get { return voteId; }
            set { voteId = value; }
        }

        public string FirstFlightDepartDate
        {
            get { return firstFlightDepartDate; }
            set { firstFlightDepartDate = value; }
        }

        public string ShoppingCartId
        {
            get { return shoppingCartId; }
            set { shoppingCartId = value; }
        }

        public string TripPlanId
        {
            get { return tripPlanId; }
            set { tripPlanId = value; }
        }

        public string MileagePlusId
        {
            get { return mileagePlusId; }
            set { mileagePlusId = value; }
        }

        public string SessionId
        {
            set { sessionId = value; }
            get { return sessionId; }
        }
    }
}
