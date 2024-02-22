using System;

namespace United.Mobile.Model.TripPlannerService
{
    [Serializable()]
    public class MOBTripPlanVoteResponse : MOBResponse
    {
        private bool isSuccess;
        private string voteId;

        public bool IsSuccess
        {
            get
            {
                return isSuccess;
            }
            set
            {
                isSuccess = value;
            }
        }

        public string VoteId
        {
            get
            {
                return voteId;
            }
            set
            {
                voteId = value;
            }
        }
    }
}
