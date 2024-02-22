using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable()]
    public class MOBBKFlightConfirmTravelerRequest : MOBRequest
    {
        public MOBBKFlightConfirmTravelerRequest()
            : base()
        {
        }

        private string sessionId = string.Empty;
        private MOBBKTraveler traveler;
        private string profileOption = string.Empty;
        private string secureTravelerOption = string.Empty;
        private bool skipSeats;
        private int paxIndex;
        private string sequenceNumber = string.Empty;

        public string SessionId
        {
            get { return this.sessionId; }
            set { this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public MOBBKTraveler Traveler
        {
            get { return this.traveler; }
            set { this.traveler = value; }
        }

        public string ProfileOption
        {
            get { return this.profileOption; }
            set { this.profileOption = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string SecureTravelerOption
        {
            get { return this.secureTravelerOption; }
            set { this.secureTravelerOption = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public bool SkipSeats
        {
            get { return this.skipSeats; }
            set { this.skipSeats = value; }
        }

        public int PaxIndex
        {
            get { return paxIndex; }
            set { paxIndex = value; }
        }

        public string SequenceNumber
        {
            get { return this.sequenceNumber; }
            set { this.sequenceNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
