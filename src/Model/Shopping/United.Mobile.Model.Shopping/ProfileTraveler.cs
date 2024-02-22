using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ProfileTraveler
    {
        private string mileagePlusNumber;
        private long customerId;
        private Person name;
        private bool isProfileOwner;
        private List<MOBAddress> addresses;

        private List<MOBEmail> emails;
        private List<MOBPaymentInfo> paymentInfos;
        private List<MOBCreditCard> creditCards;
        private List<SecureTraveler> secureTravelers;
        private List<MOBBKLoyaltyProgramProfile> airRewardPrograms;
        private string key;
        private string sharesPosition;
        private List<MOBSeat> seats;
        private List<MOBSeatPrice> seatPrices;
        private string allSeats;
        private int currentEliteLevel;
        private MOBEliteStatus eliteStatus;
        private bool isTSAFlagON;

        private List<MOBPrefAirPreference> airPreferences;
        private List<PrefContact> contacts;

        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public long CustomerId { get; set; }

        public  Person Name { get; set; }

        public bool IsProfileOwner { get; set; }

        public List<MOBAddress> Addresses { get; set; }

        public List<MOBEmail> Emails { get; set; }

        public List<PaymentInfo> PaymentInfos { get; set; }

        public List<MOBPartnerCard> PartnerCards;

        public List<MOBCreditCard> CreditCards { get; set; }

        public List<SecureTraveler> SecureTravelers { get; set; }

        public List<MOBSHOPLoyaltyProgramProfile> AirRewardPrograms { get; set; }

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string SHARESPosition
        {
            get
            {
                return this.sharesPosition;
            }
            set
            {
                this.sharesPosition = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public List<Seat> Seats { get; set; }

        public List<MOBSeatPrice> SeatPrices { get; set; }

        public string AllSeats
        {
            get
            {
                return this.allSeats;
            }
            set
            {
                if (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(this.allSeats))
                    this.allSeats = "---";
                else if (string.IsNullOrEmpty(value))
                    this.allSeats += ", " + "---";
                else if (string.IsNullOrEmpty(this.allSeats))
                    this.allSeats = value;
                else
                    this.allSeats += ", " + value;
            }
        }

        public int CurrentEliteLevel { get; set; }

        public EliteStatus EliteStatus { get; set; }

        public bool IsTSAFlagON { get; set; }

        public List<MOBPrefAirPreference> AirPreferences { get; set; }

        public List<PrefContact> Contacts { get; set; }

        public ProfileTraveler()
        {
            Addresses = new List<MOBAddress>();
            Emails = new List<MOBEmail>();
            PaymentInfos = new List<PaymentInfo>();
            CreditCards = new List<MOBCreditCard>();
            SecureTravelers = new List<SecureTraveler>();
            AirRewardPrograms = new List<MOBSHOPLoyaltyProgramProfile>();
            Seats = new List<Seat>();
            SeatPrices = new List<MOBSeatPrice>();
            AirPreferences = new List<MOBPrefAirPreference>();
            Contacts = new List<PrefContact>();
        }

    }
}
