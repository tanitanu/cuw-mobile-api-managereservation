using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Shopping;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.FormofPayment;

namespace United.Mobile.Model.Travelers
{
    [Serializable()]
    public class MOBRegisterTravelersResponse : MOBResponse
    {
        private string sessionId = string.Empty;
        private string cartId = string.Empty;
        private string token = string.Empty;
        private string profileKey = string.Empty;
        private int profileId;
        private int profileOwnerId;
        private string profileOwnerKey = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private decimal closeInBookingFee;
        private MOBSHOPReservation reservation;
        private CPProfile profile;
        private MOBShoppingCart shoppingCart;
        private bool isDefaultPaymentOption = false;
        private List<FormofPaymentOption> eligibleFormofPayments;
        private MOBSection changeInTravelersAlert;

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

        public string ProfileKey
        {
            get
            {
                return this.profileKey;
            }
            set
            {
                this.profileKey = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int ProfileId
        {
            get
            {
                return this.profileId;
            }
            set
            {
                this.profileId = value;
            }
        }

        public int ProfileOwnerId
        {
            get
            {
                return this.profileOwnerId;
            }
            set
            {
                this.profileOwnerId = value;
            }
        }

        public string ProfileOwnerKey
        {
            get
            {
                return this.profileOwnerKey;
            }
            set
            {
                this.profileOwnerKey = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public decimal CloseInBookingFee
        {
            get
            {
                return this.closeInBookingFee;
            }
            set
            {
                this.closeInBookingFee = value;
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

        public CPProfile Profile
        {
            get
            {
                return this.profile;
            }
            set
            {
                this.profile = value;
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
        public List<FormofPaymentOption> EligibleFormofPayments
        {
            get { return eligibleFormofPayments; }
            set { eligibleFormofPayments = value; }
        }
        public bool IsDefaultPaymentOption
        {
            get { return isDefaultPaymentOption; }
            set { isDefaultPaymentOption = value; }
        }

        public MOBSection ChangeInTravelersAlert
        {
            get { return changeInTravelersAlert; }
            set { changeInTravelersAlert = value; }
        }

    }
}
