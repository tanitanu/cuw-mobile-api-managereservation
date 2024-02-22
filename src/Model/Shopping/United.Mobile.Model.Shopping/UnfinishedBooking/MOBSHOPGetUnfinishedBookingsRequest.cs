using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping.UnfinishedBooking
{
    [Serializable]
    public class MOBSHOPGetUnfinishedBookingsRequest : MOBSHOPUnfinishedBookingRequestBase 
    {
        #region Variables

        private List<MOBSHOPUnfinishedBooking> unfinishedBookings = new List<MOBSHOPUnfinishedBooking>();
        private bool isCatalogOnForTavelerTypes;

        #endregion

        #region Properties

        public List<MOBSHOPUnfinishedBooking> UnfinishedBookings
        {
            get { return unfinishedBookings; }
            set { unfinishedBookings = value; }
        }

        public bool IsCatalogOnForTavelerTypes
        {
            get { return isCatalogOnForTavelerTypes; }
            set { isCatalogOnForTavelerTypes = value; }
        }

        #endregion
    }
}
