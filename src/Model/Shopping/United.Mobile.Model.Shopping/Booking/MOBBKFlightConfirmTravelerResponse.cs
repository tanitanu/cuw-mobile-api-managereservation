using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
//using United.Reward.Configuration;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable()]
    public class MOBBKFlightConfirmTravelerResponse : MOBResponse
    {
        public MOBBKFlightConfirmTravelerResponse()
            : base()
        {
            this.rewardPrograms = new List<MOBBKRewardProgram>();
            ConfigurationParameter.ConfigParameter parm = ConfigurationParameter.ConfigParameter.Configuration;
            for (int i = 0; i < parm.RewardTypes.Count; i++)
            {
                MOBBKRewardProgram p = new MOBBKRewardProgram();
                p.Type = parm.RewardTypes[i].Type;
                p.Description = parm.RewardTypes[i].Description;
                rewardPrograms.Add(p);
            }
        }
        
        private string sessionId = string.Empty;

        private MOBBKFlightConfirmTravelerRequest flightConfirmTravelerRequest;
        private MOBBKTraveler currentTraveler;
        private List<MOBBKTraveler> travelers;
        private List<MOBBKTrip> trips;
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
        private string phoneNumberDisclaimer = ConfigurationManager.AppSettings["PhoneNumberDisclaimer"];
        private List<MOBTypeOption> disclaimer ;
        private List<MOBBKRewardProgram> rewardPrograms;
        private MOBBKReservation reservation;
        private List<MOBTypeOption> hazMat;
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

        public MOBBKFlightConfirmTravelerRequest FlightConfirmTravelerRequest
        {
            get { return this.flightConfirmTravelerRequest; }
            set { this.flightConfirmTravelerRequest = value; }
        }

        public MOBBKTraveler CurrentTraveler
        {
            get { return this.currentTraveler; }
            set { this.currentTraveler = value; }
        }

        public List<MOBBKTraveler> Travelers
        {
            get { return this.travelers; }
            set { this.travelers = value; }
        }


        public List<MOBBKTrip> Trips
        {
            get
            {
                return this.trips;
            }
            set
            {
                this.trips = value;
            }
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
                string rText = System.Configuration.ConfigurationManager.AppSettings["DOTBagRules"];
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

                return this.dotBagRules; 
            }
            set 
            {
                this.dotBagRules = value;
            }
        }

        public string PhoneNumberDisclaimer
        {
            get
            {
                return this.phoneNumberDisclaimer;
            }
            set
            {
            }
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

        public List<MOBBKRewardProgram> RewardPrograms
        {
            get { return this.rewardPrograms; }
            set { this.rewardPrograms = value; }
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
