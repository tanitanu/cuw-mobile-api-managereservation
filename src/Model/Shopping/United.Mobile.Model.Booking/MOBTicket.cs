using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBTicket
    {
        private string number = string.Empty;
        private int sequence;
        private string status = string.Empty;
        private string issuedTo = string.Empty;
        private string issuedBy = string.Empty;
        private string origin;
        private string destination;
        private string flightDate;
        private bool isActive;
        private bool isBulkTicket;
        private bool isGiftTicket;
        private int giftSequence;
        private string fareBasis = string.Empty;
        private decimal totalFare;
        private string currencyOfIssuance = string.Empty;

        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int Sequence
        {
            get
            {
                return this.sequence;
            }
            set
            {
                this.sequence = value;
            }
        }

        public string Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string IssuedTo
        {
            get
            {
                return this.issuedTo;
            }
            set
            {
                this.issuedTo = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string IssuedBy
        {
            get
            {
                return this.issuedBy;
            }
            set
            {
                this.issuedBy = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Destination
        {
            get
            {
                return this.destination;
            }
            set
            {
                this.destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string FlightDate
        {
            get
            {
                return this.flightDate;
            }
            set
            {
                this.flightDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                this.isActive = value;
            }
        }

        public bool IsBulkTicket
        {
            get
            {
                return this.isBulkTicket;
            }
            set
            {
                this.isBulkTicket = value;
            }
        }

        public bool IsGiftTicket
        {
            get
            {
                return this.isGiftTicket;
            }
            set
            {
                this.isGiftTicket = value;
            }
        }

        public int GiftSequence
        {
            get
            {
                return this.giftSequence;
            }
            set
            {
                this.giftSequence = value;
            }
        }

        public string FareBasis
        {
            get
            {
                return this.fareBasis;
            }
            set
            {
                this.fareBasis = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public decimal TotalFare
        {
            get
            {
                return this.totalFare;
            }
            set
            {
                this.totalFare = value;
            }
        }

        public string CurrencyOfIssuance
        {
            get
            {
                return this.currencyOfIssuance;
            }
            set
            {
                this.currencyOfIssuance = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

    }
}
