using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class LDPassenger
    {
        private string recordLocator = string.Empty;
        private MOBName name;
        private string seat = string.Empty;
        private string standbyPassCode = string.Empty;
        private string classDescription = string.Empty;
        private SeatLinkData showSeatAsLink;
        private bool isCheckedIn;

        public bool IsCheckedIn
        {
            get
            {
                return this.isCheckedIn;
            }
            set
            {
                this.isCheckedIn = value;
            }
        }

        public SeatLinkData ShowSeatAsLink
        {
            get
            {
                return showSeatAsLink;
            }
            set
            {
                showSeatAsLink = value;
            }
        }

        public string RecordLocator
        {
            get { return this.recordLocator; }
            set
            {
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public MOBName Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
            }
        }

        public string Seat
        {
            get { return this.seat; }
            set
            {
                this.seat = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string StandbyPassCode
        {
            get { return this.standbyPassCode; }
            set
            {
                this.standbyPassCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string ClassDescription
        {
            get
            {
                return classDescription;
            }
            set
            {
                classDescription = value;
            }
        }
    }

    [Serializable]
    public class SeatLinkData
    {
        private string recordLocator = string.Empty;
        private string firstName = string.Empty;
        private string lastName = string.Empty;

        public string FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                this.firstName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string RecordLocator
        {
            get { return this.recordLocator; }
            set
            {
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

    }
}
