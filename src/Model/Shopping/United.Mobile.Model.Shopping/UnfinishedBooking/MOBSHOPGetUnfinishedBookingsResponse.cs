using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping.UnfinishedBooking
{
    [Serializable]
    public class MOBSHOPGetUnfinishedBookingsResponse : MOBResponse
    {
        private List<MOBSHOPUnfinishedBooking> pricedUnfinishedBookingList;

        public List<MOBSHOPUnfinishedBooking> PricedUnfinishedBookingList
        {
            get { return pricedUnfinishedBookingList; }
            set { pricedUnfinishedBookingList = value; }
        }
    }
}
