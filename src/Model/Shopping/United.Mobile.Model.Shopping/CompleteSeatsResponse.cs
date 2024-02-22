using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CompleteSeatsResponse : MOBResponse
    {
        public string SessionId { get; set; } = string.Empty;
       
        public CompleteSeatsRequest FlightCompleteSeatsRequest { get; set; } 
        public List<MOBCPTraveler> Travelers { get; set; } 

        //public FlightAddTripResponse FlightAddTripResponse
        //{
        //    get { return this.flightAddTripResponse; }
        //    set { this.flightAddTripResponse = value; }
        //}

        public List<MOBCreditCard> CreditCards { get; set; } 

        public List<Seat> Seats { get; set; } 

        public List<MOBSeatPrice> SeatPrices { get; set; } 
        public List<MOBAddress> ProfileOwnerAddresses { get; set; } 

        public List<MOBEmail> Emails { get; set; } 
        public List<string> TermsAndConditions { get; set; } 
        public MOBSHOPReservation Reservation { get; set; } 

        public List<MOBTypeOption> HazMat { get; set; }

        public string FooterMessage { get; set; } = string.Empty;
       
        public List<MOBTypeOption> Disclaimer { get; set; } 

        public string ContractOfCarriage { get; set; } = string.Empty;
        
       
        public CompleteSeatsResponse()
        {
            Travelers = new List<MOBCPTraveler>();
            CreditCards = new List<MOBCreditCard>();
            Seats = new List<Seat>();
            SeatPrices = new List<MOBSeatPrice>();
            ProfileOwnerAddresses = new List<MOBAddress>();
            Emails = new List<MOBEmail>();
            HazMat = new List<MOBTypeOption>();
            Disclaimer = new List<MOBTypeOption>();
        }
    }
}
