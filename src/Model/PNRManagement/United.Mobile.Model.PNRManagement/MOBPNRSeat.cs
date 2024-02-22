using System;

namespace United.Mobile.Model.PNRManagement
{
    [Serializable]
    public class MOBPNRSeat
    {
        private string number = string.Empty;
        private string seatRow = string.Empty;
        private string seatLetter = string.Empty;
        private string passengerSHARESPosition = string.Empty;

        private string seatStatus = string.Empty;
        private string segmentIndex = string.Empty;
        private string origin = string.Empty;
        private string destination = string.Empty;
        private string eddNumber = string.Empty;
        private string eDocId = string.Empty;
        private double price;
        private string currency = string.Empty; 
        private string programCode = string.Empty;

        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string SeatRow
        {
            get
            {
                return this.seatRow;
            }
            set
            {
                this.seatRow = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string SeatLetter
        {
            get
            {
                return this.seatLetter;
            }
            set
            {
                this.seatLetter = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string PassengerSHARESPosition
        {
            get
            {
                return this.passengerSHARESPosition;
            }
            set
            {
                this.passengerSHARESPosition = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string SeatStatus
        {
            get
            {
                return this.seatStatus;
            }
            set
            {
                this.seatStatus = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string SegmentIndex
        {
            get
            {
                return this.segmentIndex;
            }
            set
            {
                this.segmentIndex = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string EddNumber
        {
            get
            {
                return this.eddNumber;
            }
            set
            {
                this.eddNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EDocId
        {
            get
            {
                return this.eDocId;
            }
            set
            {
                this.eDocId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public double Price
        {
            get
            {
                return this.price;
            }
            set
            {
                this.price = value;
            }
        }

        public string Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string ProgramCode
        {
            get
            {
                return this.programCode;
            }
            set
            {
                this.programCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
