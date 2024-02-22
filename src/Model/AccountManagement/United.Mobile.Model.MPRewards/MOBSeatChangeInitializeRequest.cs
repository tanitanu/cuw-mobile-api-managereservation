using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.MPRewards
{
    [Serializable()]
    public class MOBSeatChangeInitializeRequest : MOBRequest
    {
        private string sessionId;
        private string cartId = string.Empty;
        private string flow = string.Empty;
        private string recordLocator = string.Empty;
        private string lastName = string.Empty;
        private string offersRequestData = null;
        private MOBSeatFocus seatFocusRequest = null;
        private List<MOBItem> catalogValues;
        private MOBTravelerSignInData travelerSignInData = null;
        private string requestType = string.Empty;

        public List<MOBItem> CatalogValues
        {
            get
            {
                return this.catalogValues;
            }
            set
            {
                this.catalogValues = value;
            }
        }

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
        public string CartId
        {
            get
            {
                return this.cartId;
            }
            set
            {
                this.cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Flow
        {
            get
            {
                return this.flow;
            }
            set
            {
                this.flow = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
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


        public string OffersRequestData
        {
            get { return offersRequestData; }
            set { offersRequestData = value; }
        }

        public MOBSeatFocus SeatFocusRequest
        {
            get { return seatFocusRequest; }
            set { seatFocusRequest = value; }
        }

        public MOBTravelerSignInData TravelerSignInData
        {
            get { return travelerSignInData; }
            set { travelerSignInData = value; }
        }

        public string RequestType
        {
            get
            {
                return this.requestType;
            }
            set
            {
                this.requestType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
