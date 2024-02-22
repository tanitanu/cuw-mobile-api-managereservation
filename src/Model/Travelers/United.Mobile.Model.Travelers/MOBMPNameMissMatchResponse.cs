using System;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.Travelers

{
    [Serializable()]
    public class MOBMPNameMissMatchResponse : MOBResponse
    {
        private string sessionId = string.Empty;
        private string cartId = string.Empty;
        private string token = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private CPTraveler traveler;
        private bool IsMPNameMisMatch = false;
        private MOBSHOPReservation reservation;
        private MOBShoppingCart shoppingCart;

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

        public CPTraveler Traveler
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

        public bool isMPNameMisMatch
        {
            get
            {
                return this.IsMPNameMisMatch;
            }
            set
            {
                this.IsMPNameMisMatch = value;
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

        public MOBShoppingCart ShoppingCart
        {
            get
            {
                return this.shoppingCart;
            }
            set
            {
                this.shoppingCart = value;
            }
        }
    }
}
