using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBTraveler
    {
        private string mileagePlusNumber;
        private long customerId;
        private MOBName name;
        private bool isProfileOwner;
        private List<MOBAddress> addresses;
        private List<MOBPhone> phones;
        private List<MOBEmail> emails;
        private List<MOBPaymentInfo> paymentInfos;
        private List<MOBPartnerCard> partnerCards;
        private List<MOBCreditCard> creditCards;
        private List<MOBSecureTraveler> secureTravelers;
        private List<MOBAirRewardProgram> airRewardPrograms;
        private string key;
        private string sharesPosition;
        private List<MOBSeat> seats;
        private List<MOBSeatPrice> seatPrices;
        private string allSeats;
        private int currentEliteLevel;
        private MOBEliteStatus eliteStatus;
        private bool isTSAFlagON;

        private List<United.Mobile.Model.MPAuthentication.MOBPrefAirPreference> airPreferences;
        private List<United.Mobile.Model.MPAuthentication.MOBPrefContact> contacts;

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

        [XmlIgnore]
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

        public MOBName Name
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

        public List<MOBPhone> Phones
        {
            get
            {
                return this.phones;
            }
            set
            {
                this.phones = value;
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

        public List<MOBPaymentInfo> PaymentInfos
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

        public List<MOBPartnerCard> PartnerCards
        {
            get
            {
                return this.partnerCards;
            }
            set
            {
                this.partnerCards = value;
            }
        }

        public List<MOBCreditCard> CreditCards
        {
            get { return this.creditCards; }
            set { this.creditCards = value; }
        }

        public List<MOBSecureTraveler> SecureTravelers
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

        public List<MOBAirRewardProgram> AirRewardPrograms
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

        public List<MOBSeat> Seats
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


        public List<MOBPrefContact> Contacts
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
