using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.Booking;
using United.Mobile.Model.Shopping.Misc;
using MOBBKTrip = United.Mobile.Model.Shopping.Booking.MOBBKTrip;
using MOBSeatMap = United.Mobile.Model.Shopping.MOBSeatMap;

namespace United.Mobile.Model.MPRewards
{
    [Serializable()]
    public class MOBSeatChangeInitializeResponse : MOBResponse
    {
        [JsonIgnore]
        private IConfiguration _configuration;
        public string ObjectName = "United.Definition.MOBSeatChangeInitializeResponse";
        private string sessionId = string.Empty;
        private string flow = string.Empty;
        private MOBSeatChangeInitializeRequest request;
        private MOBBKTraveler currentTravelerInfo;
        private List<MOBBKTraveler> bookingTravlerInfo;
        private List<MOBBKTrip> selectedTrips;
        private List<MOBCreditCard> creditCards;
        private bool finished;
        private bool isLastTraveler;
        private List<MOBSeatMap> seatMap;
        private List<MOBEmail> emails;
        private List<MOBAddress> profileOwnerAddresses;
        private List<MOBTypeOption> exitAdvisory;
        private List<TripSegment> segments;
        private List<string> termsAndConditions;
        private List<string> dotBagRules;
        private List<Seat> seats;
        private bool isVerticalSeatMapEnabled = false;

        private MOBOnScreenAlert unsavedSeatChangeAlert;

        public MOBOnScreenAlert UnsavedSeatChangeAlert
        {
            get { return unsavedSeatChangeAlert; }
            set { unsavedSeatChangeAlert = value; }
        }

        private string interlineErrorMessage;

        public string InterlineErrorMessage
        {
            get { return interlineErrorMessage; }
            set { interlineErrorMessage = value; }
        }


        private string continueButtonText;
        public string ContinueButtonText
        {
            get { return this.continueButtonText; }
            set { this.continueButtonText = value; }
        }

        private string isCheckedInChangeSeatEligible;
        public string IsCheckedInChangeSeatEligible
        {
            get { return this.isCheckedInChangeSeatEligible; }
            set { this.isCheckedInChangeSeatEligible = value; }
        }


        public MOBSeatChangeInitializeRequest Request
        {
            get { return this.request; }
            set { this.request = value; }
        }
        public string Flow
        {
            get
            {
                return this.flow;
            }
            set
            {
                this.flow = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
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

        public MOBBKTraveler CurrentTravelerInfo
        {
            get { return this.currentTravelerInfo; }
            set { this.currentTravelerInfo = value; }
        }

        public List<MOBBKTraveler> BookingTravlerInfo
        {
            get { return this.bookingTravlerInfo; }
            set { this.bookingTravlerInfo = value; }
        }

        //public List<MOBBKTraveler> BookingTravlerInfo
        //{
        //    get { return this.bookingTravlerInfo; }
        //    set { this.bookingTravlerInfo = value; }
        //}

        public List<MOBBKTrip> SelectedTrips
        {
            get { return this.selectedTrips; }
            set { this.selectedTrips = value; }
        }

        public List<MOBCreditCard> CreditCards
        {
            get { return this.creditCards; }
            set { this.creditCards = value; }
        }

        public bool Finished
        {
            get { return this.finished; }
            set { this.finished = value; }
        }

        public bool IsLastTraveler
        {
            get { return this.isLastTraveler; }
            set { this.isLastTraveler = value; }
        }

        public List<MOBSeatMap> SeatMap
        {
            get { return this.seatMap; }
            set { this.seatMap = value; }
        }

        public List<MOBEmail> Emails
        {
            get { return this.emails; }
            set { this.emails = value; }
        }

        public List<MOBAddress> ProfileOwnerAddresses
        {
            get { return this.profileOwnerAddresses; }
            set { this.profileOwnerAddresses = value; }
        }

        public List<MOBTypeOption> ExitAdvisory
        {
            get { return this.exitAdvisory; }
            set { this.exitAdvisory = value; }
        }

        public List<TripSegment> Segments
        {
            get { return this.segments; }
            set { this.segments = value; }
        }

        public List<string> TermsAndConditions
        {
            get { return this.termsAndConditions; }
            set { this.termsAndConditions = value; }
        }

        public List<string> DOTBagRules
        {
            get
            {
                if (_configuration != null)
                {
                    string rText = _configuration.GetValue<string>("DOTBagRules");
                    if (!string.IsNullOrEmpty(rText))
                    {
                        string[] rules = rText.Split('|');
                        if (rules != null && rules.Length > 0)
                        {
                            this.dotBagRules = new List<string>();
                            foreach (string s in rules)
                            {
                                this.dotBagRules.Add(s);
                            }
                        }
                    }
                }
                return this.dotBagRules;
            }
            set
            {
                this.dotBagRules = value;
            }
        }

        public List<Seat> Seats
        {
            get
            {
                return this.seats;
            }
            set
            {
                this.seats = value;
            }
        }
        private string phoneNumberDisclaimer;

        public  string PhoneNumberDisclaimer
        {
            get
            {
                return this.phoneNumberDisclaimer;
            }
            set
            {
            }
        }

        public bool IsVerticalSeatMapEnabled
        {
            get { return this.isVerticalSeatMapEnabled; }
            set { this.isVerticalSeatMapEnabled = value; }
        }

        public MOBSeatChangeInitializeResponse(IConfiguration configuration)
        {
            _configuration = configuration;
            phoneNumberDisclaimer = _configuration.GetValue<string>("PhoneNumberDisclaimer");
            

        }
        public MOBSeatChangeInitializeResponse()
        {

        }
    }
}
