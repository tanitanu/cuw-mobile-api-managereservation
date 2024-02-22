using System;
using United.Persist;

namespace United.Mobile.Model.Shopping.UnfinishedBooking
{
    [Serializable]
    public class MOBSHOPSelectUnfinishedBookingRequest : MOBSHOPUnfinishedBookingRequestBase
    {
        private MOBSHOPUnfinishedBooking selectedUnfinishBooking;
        public MOBSHOPUnfinishedBooking SelectedUnfinishBooking
        {
            get { return selectedUnfinishBooking; }
            set { selectedUnfinishBooking = value; }
        }
    }
}
