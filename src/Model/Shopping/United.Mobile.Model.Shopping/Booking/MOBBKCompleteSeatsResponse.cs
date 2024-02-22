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
    public class MOBBKCompleteSeatsResponse : MOBResponse
    {
        public MOBBKCompleteSeatsResponse()
            : base()
        {
        }
        private string sessionId = string.Empty;
        private MOBBKCompleteSeatsRequest flightCompleteSeatsRequest;
        private List<MOBBKTraveler> travelers;
        //private FlightAddTripResponse flightAddTripResponse;
        private List<MOBCreditCard> creditCards;
        private List<Seat> seats;
        private List<MOBSeatPrice> seatPrices;
        private List<MOBAddress> profileOwnerAddresses;
        private List<MOBEmail> emails;
        private List<string> termsAndConditions;
        private MOBBKReservation reservation;
        private List<MOBTypeOption> hazMat;
        private List<MOBTypeOption> disclaimer;
        private string contractOfCarriage = string.Empty;

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

        public MOBBKCompleteSeatsRequest FlightCompleteSeatsRequest
        {
            get { return this.flightCompleteSeatsRequest; }
            set { this.flightCompleteSeatsRequest = value; }
        }

        public List<MOBBKTraveler> Travelers
        {
            get { return this.travelers; }
            set { this.travelers = value; }
        }

        //public FlightAddTripResponse FlightAddTripResponse
        //{
        //    get { return this.flightAddTripResponse; }
        //    set { this.flightAddTripResponse = value; }
        //}

        public List<MOBCreditCard> CreditCards
        {
            get { return this.creditCards; }
            set { this.creditCards = value; }
        }

        public List<Seat> Seats
        {
            get { return this.seats; }
            set { this.seats = value; }
        }

        public List<MOBSeatPrice> SeatPrices
        {
            get { return this.seatPrices; }
            set { this.seatPrices = value; }
        }

        public List<MOBAddress> ProfileOwnerAddresses
        {
            get { return this.profileOwnerAddresses; }
            set { this.profileOwnerAddresses = value; }
        }

        public List<MOBEmail> Emails
        {
            get { return this.emails; }
            set { this.emails = value; }
        }

        public List<string> TermsAndConditions
        {
            get { return this.termsAndConditions; }
            set { this.termsAndConditions = value; }
        }

        public MOBBKReservation Reservation
        {
            get { return this.reservation; }
            set { this.reservation = value; }
        }

        public List<MOBTypeOption> HazMat
        {
            get { return this.hazMat; }
            set { this.hazMat = value; }
        }

        private string footerMessage = null;

        public string FooterMessage
        {
            get { return footerMessage; }
            set { footerMessage = value; }
        }

        public List<MOBTypeOption> Disclaimer
        {
            get
            {
                return this.disclaimer;
            }
            set
            {
                this.disclaimer = value;
            }
        }

        public string ContractOfCarriage
        {
            get
            {
                return this.contractOfCarriage;
            }
            set
            {
                this.contractOfCarriage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
