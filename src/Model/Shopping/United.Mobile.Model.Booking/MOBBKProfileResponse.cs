using System;
using System.Collections.Generic;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBBKProfileResponse : MOBResponse
    {
        private List<MOBBKProfile> profiles;
        private string sessionId = string.Empty;
        private MOBSHOPReservation reservation;
        private List<MOBItem> insertUpdateKeys;
        private string cartId = string.Empty;
        private string token = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private bool isMPNameMisMatch = false;
        private MOBCPTraveler traveler;
        private MOBShoppingCart shoppingCart;
        private MOBProfileRequest request;

        public MOBProfileRequest Request
        {
            get
            {
                return this.request;
            }
            set
            {
                this.request = value;
            }
        }

        public List<MOBBKProfile> Profiles
        {
            get
            {
                return profiles;
            }
            set
            {
                this.profiles = value;
            }
        }
        public string SessionId
        {
            get
            {
                return sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }       
        public MOBSHOPReservation Reservation
        {
            get
            {
                return this.reservation;
            }
            set
            {
                this.reservation = value;
            }
        }

        public List<MOBItem> InsertUpdateKeys
        {
            get
            {
                return this.insertUpdateKeys;
            }
            set
            {
                this.insertUpdateKeys = value;
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
                this.cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                this.token = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string MileagePlusNumber
        {
            get
            {
                return mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
        public bool ISMPNameMisMatch
        {
            get
            {
                return this.isMPNameMisMatch;
            }
            set
            {
                this.isMPNameMisMatch = value;
            }
        }
        public MOBCPTraveler Traveler
        {
            get
            {
                return this.traveler;
            }
            set
            {
                this.traveler = value;
            }
        }
        public MOBShoppingCart ShoppingCart
        {
            get { return shoppingCart; }
            set { shoppingCart = value; }
        }
    }
}
