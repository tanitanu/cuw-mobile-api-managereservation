using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBEmpSeat
    {
        private string seatAssignment;
        private string oldSeatAssignment;
        private string attribute;
        private string origin;
        private string destination;
        private string flightNumber;
        private string departureDate;
        private string travelerSharesIndex;
        private int key;
        private bool uaOperated;
        private decimal price;
        private decimal priceAfterTravelerCompanionRules;
        private string currency;
        private string programCode;
        private string seatType;

        private decimal oldSeatPrice;
        private string oldSeatCurrency;
        private string oldSeatProgramCode;
        private string oldSeatType;
        private string oldSeatEDDTransactionId;

        private bool isEPA;
        private bool isEPAFreeCompanion;
        private string seatFeature;

        private bool limitedRecline;
        
        public bool LimitedRecline
        {
            get { return limitedRecline; }
            set { limitedRecline = value; }
        }

        public string SeatAssignment
        {
            get
            {
                return this.seatAssignment;
            }
            set
            {
                this.seatAssignment = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string OldSeatAssignment
        {
            get
            {
                return this.oldSeatAssignment;
            }
            set
            {
                this.oldSeatAssignment = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Attribute
        {
            get
            {
                return this.attribute;
            }
            set
            {
                this.attribute = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Origin
        {
            get { return this.origin; }
            set { this.origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string Destination
        {
            get { return this.destination; }
            set { this.destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string FlightNumber
        {
            get { return this.flightNumber; }
            set { this.flightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string DepartureDate
        {
            get { return this.departureDate; }
            set { this.departureDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string TravelerSharesIndex
        {
            get { return this.travelerSharesIndex; }
            set { this.travelerSharesIndex = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public int Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        public bool UAOperated
        {
            get { return this.uaOperated; }
            set { this.uaOperated = value; }
        }

        public decimal Price
        {
            get { return this.price; }
            set { this.price = value; }
        }

        public decimal PriceAfterTravelerCompanionRules
        {
            get { return this.priceAfterTravelerCompanionRules; }
            set { this.priceAfterTravelerCompanionRules = value; }
        }

        public string Currency
        {
            get { return this.currency; }
            set { this.currency = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string ProgramCode
        {
            get { return this.programCode; }
            set { this.programCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string SeatType
        {
            get { return this.seatType; }
            set { this.seatType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public decimal OldSeatPrice
        {
            get { return this.oldSeatPrice; }
            set { this.oldSeatPrice = value; }
        }

        public string OldSeatCurrency
        {
            get { return this.oldSeatCurrency; }
            set { this.oldSeatCurrency = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string OldSeatProgramCode
        {
            get { return this.oldSeatProgramCode; }
            set { this.oldSeatProgramCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string OldSeatType
        {
            get { return this.oldSeatType; }
            set { this.oldSeatType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string OldSeatEDDTransactionId
        {
            get { return this.oldSeatEDDTransactionId; }
            set { this.oldSeatEDDTransactionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public bool IsEPA
        {
            get { return this.isEPA; }
            set { this.isEPA = value; }
        }

        public bool IsEPAFreeCompanion
        {
            get { return this.isEPAFreeCompanion; }
            set { this.isEPAFreeCompanion = value; }
        }

        public string SeatFeature
        {
            get
            {
                return this.seatFeature;
            }
            set
            {
                this.seatFeature = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
