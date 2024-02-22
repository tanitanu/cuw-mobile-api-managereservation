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
    public class MOBBKProfileTraveler
    {
        private string mileagePlusNumber;
        private long customerId;
        private MOBBKPerson name;
        private bool isProfileOwner;
        private List<MOBAddress> addresses;
       
        private List<MOBEmail> emails;
        private List<PaymentInfo> paymentInfos;
        private List<MOBCreditCard> creditCards;
        private List<SecureTraveler> secureTravelers;
        private List<MOBBKLoyaltyProgramProfile> airRewardPrograms;
        private string key;
        private string sharesPosition;
        private List<Seat> seats;
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

        
        public long CustomerId
        {
            get
            {
                return this.customerId;
            }
            set
            {
                this.customerId = value;
            }
        }

        public MOBBKPerson Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public bool IsProfileOwner
        {
            get
            {
                return this.isProfileOwner;
            }
            set
            {
                this.isProfileOwner = value;
            }
        }

        public List<MOBAddress> Addresses
        {
            get
            {
                return this.addresses;
            }
            set
            {
                this.addresses = value;
            }
        }

      

        public List<MOBEmail> Emails
        {
            get
            {
                return this.emails;
            }
            set
            {
                this.emails = value;
            }
        }

        public List<PaymentInfo> PaymentInfos
        {
            get
            {
                return this.paymentInfos;
            }
            set
            {
                this.paymentInfos = value;
            }
        }

        public List<MOBCreditCard> CreditCards
        {
            get { return this.creditCards; }
            set { this.creditCards = value; }
        }

        public List<SecureTraveler> SecureTravelers
        {
            get
            {
                return this.secureTravelers;
            }
            set
            {
                this.secureTravelers = value;
            }
        }

        public List<MOBBKLoyaltyProgramProfile> AirRewardPrograms
        {
            get
            {
                return this.airRewardPrograms;
            }
            set
            {
                this.airRewardPrograms = value;
            }
        }

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

        public List<MOBSeatPrice> SeatPrices
        {
            get
            {
                return this.seatPrices;
            }
            set
            {
                this.seatPrices = value;
            }
        }

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

        public int CurrentEliteLevel
        {
            get { return this.currentEliteLevel; }
            set { this.currentEliteLevel = value; }
        }

        public MOBEliteStatus EliteStatus
        {
            get { return this.eliteStatus; }
            set { this.eliteStatus = value; }
        }

        public bool IsTSAFlagON
        {
            get
            {
                return this.isTSAFlagON;
            }
            set
            {
                this.isTSAFlagON = value;
            }
        }

        public List<MOBPrefAirPreference> AirPreferences
        {
            get
            {
                return this.airPreferences;
            }
            set
            {
                this.airPreferences = value;
            }
        }


        public List<PrefContact> Contacts
        {
            get
            {
                return this.contacts;
            }
            set
            {
                this.contacts = value;
            }
        }
    }
}
