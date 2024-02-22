using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FareRulesResponse : MOBResponse
    {
        private string sessionId = string.Empty;

        private MOBSHOPReservation reservation;

        public string SessionId
        {
            get
            {
                return sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBSHOPReservation Reservation
        {
            get
            {
                return this.reservation;
            }
            set
            {
                this.reservation = value;
            }
        }
    }
}
