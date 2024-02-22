using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBBKAvailability
    {
        private string sessionId = string.Empty;
        private string cartId = string.Empty;
        private decimal closeInBookingFee;
        private MOBBKTrip trip;
        private MOBBKReservation reservation;

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CartId
        {
            get
            {
                return this.cartId;
            }
            set
            {
                this.cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public decimal CloseInBookingFee
        {
            get
            {
                return this.closeInBookingFee;
            }
            set
            {
                this.closeInBookingFee = value;
            }
        }

        public MOBBKTrip Trip
        {
            get
            {
                return this.trip;
            }
            set
            {
                this.trip = value;
            }
        }

        public MOBBKReservation Reservation
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
