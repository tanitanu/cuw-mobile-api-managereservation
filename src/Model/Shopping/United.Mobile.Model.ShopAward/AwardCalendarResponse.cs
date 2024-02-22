using System;

namespace United.Mobile.Model.Shopping.AwardCalendar
{
    [Serializable()]
    public class AwardCalendarResponse : MOBResponse
    {
        private SelectTripRequest awardCalendarRequest;
        private string cartId = string.Empty;
        private string sessionID = string.Empty;
        private AwardDynamicCalendar awardDynamicCalendar;
        private string callDurationText;

        public SelectTripRequest AwardCalendarRequest
        {
            get
            {
                return this.awardCalendarRequest;
            }
            set
            {
                this.awardCalendarRequest = value;
            }
        }
        public string CartId
        {
            get { return cartId; }
            set { cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
        public string SessionID
        {
            get
            {
                return this.sessionID;
            }
            set
            {
                this.sessionID = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public AwardDynamicCalendar AwardDynamicCalendar
        {
            get
            {
                return awardDynamicCalendar;
            }
            set
            {
                awardDynamicCalendar = value;
            }
        }
        public string CallDurationText
        {
            get { return callDurationText; }
            set { callDurationText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
