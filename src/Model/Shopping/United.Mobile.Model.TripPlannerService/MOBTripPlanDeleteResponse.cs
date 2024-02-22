using System;

namespace United.Mobile.Model.TripPlannerService
{
    [Serializable()]
    public class MOBTripPlanDeleteResponse : MOBResponse
    {
        private bool isSuccess;

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
    }
}
