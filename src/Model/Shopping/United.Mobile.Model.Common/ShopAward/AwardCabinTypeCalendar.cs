using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{

    [Serializable()]
    public class AwardCabinTypeCalendar
    {
        private List<AwardCalendarDay> calendarWeek;

        public List<AwardCalendarDay> CalendarWeek
        {
            get { return calendarWeek; }
            set { calendarWeek = value; }
        }
        private string cabinType;
        public string CabinType
        {
            get { return cabinType; }
            set { cabinType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private bool isDefaultSelected;

        public bool IsDefaultSelected
        {
            get { return isDefaultSelected; }
            set { isDefaultSelected = value; }
        }
    }
}
