using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using United.Definition;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping.Booking;
//using United.Definition.Booking;
using United.Mobile.Model.Shopping.Misc;
using United.Persist;

namespace United.Mobile.Model.Shopping
{
    public class SeatChangeState : PersistBase, IPersist
    {
        public SeatChangeState(string json, string objectType)
        {
            Json = json;
            ObjectType = objectType;
        }

        #region IPersist Members

        public string ObjectName { get; set; } = "United.Persist.Definition.SeatChange.SeatChangeState";

        #endregion

        private string recordLocator = string.Empty;
        public string RecordLocator
        {
            get
            {
                return this.recordLocator;
            }
            set
            {
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        private string lastName = string.Empty;
        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        private string pnrCreationDate = string.Empty;
        public string PNRCreationDate
        {
            get
            {
                return this.pnrCreationDate;
            }
            set
            {
                this.pnrCreationDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string sessionId = string.Empty;
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

        private string cartId = string.Empty;
        public string CartId
        {
            get
            {
                return this.cartId;
            }
            set
            {
                this.cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBBKTraveler> BookingTravelerInfo { get; set; }

        public List<MOBBKTrip> Trips { get; set; }

        [XmlArrayItem("MOBSeat")]
        public List<Seat> Seats { get; set; } 

        public List<MOBSeatPrice> SeatPrices { get; set; } 

        public List<MOBCreditCard> CreditCards { get; set; } 

        public List<MOBAddress> ProfileOwnerAddresses { get; set; }

        public List<MOBEmail> Emails { get; set; }

        public int TotalEplusEligible { get; set; }

        [XmlArrayItem("MOBTripSegment")]
        public List<TripSegment> Segments { get; set; }

        public MOBTravelerSignInData TravelerSignInData { get; set; }         

        public SeatChangeState()
        {
            BookingTravelerInfo = new List<MOBBKTraveler>();
            Trips = new List<MOBBKTrip>();
            Seats = new List<Seat>();
            SeatPrices = new List<MOBSeatPrice>();
            CreditCards = new List<MOBCreditCard>();
            ProfileOwnerAddresses = new List<MOBAddress>();
            Emails = new List<MOBEmail>();
            Segments = new List<TripSegment>();
        }
    }
}
