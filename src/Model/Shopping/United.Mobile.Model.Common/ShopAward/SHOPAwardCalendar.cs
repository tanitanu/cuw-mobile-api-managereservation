using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class SHOPAwardCalendar
    {
        private List<AwardCalendarItem> awardCalendarItems;
        private string productId = string.Empty;
        private string tripId = string.Empty;

        public List<AwardCalendarItem> AwardCalendarItems
        {
            get
            {
                return this.awardCalendarItems;
            }
            set
            {
                this.awardCalendarItems = value;
            }
        }


        public string ProductId
        {
            get
            {
                return this.productId;
            }
            set
            {
                this.productId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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
    }
}
