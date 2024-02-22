using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable()]
    public class MOBBKSelectSeatsResponse : MOBResponse
    {
        private MOBBKSelectSeatsRequest flightTravelerSeatsRequest;
        private List<Seat> seats;
        private List<MOBSeatMap> seatMap;
        private List<MOBTypeOption> exitAdvisory;
        private string sessionId = string.Empty;

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
        public MOBBKSelectSeatsRequest FlightTravelerSeatsRequest
        {
            get { return this.flightTravelerSeatsRequest; }
            set { this.flightTravelerSeatsRequest = value; }
        }

        public List<Seat> Seats
        {
            get { return this.seats; }
            set { seats = value; }
        }

        public List<MOBSeatMap> SeatMap
        {
            get { return this.seatMap; }
            set { this.seatMap = value; }
        }

        public List<MOBTypeOption> ExitAdvisory
        {
            get { return this.exitAdvisory; }
            set { this.exitAdvisory = value; }
        }
    }
}
