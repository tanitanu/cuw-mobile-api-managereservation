using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBUpdateTravelerRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string token = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private bool updateTravelerBasiInfo;
        private bool updatePhoneInfo;
        private bool updateEmailInfo;
        private bool updateRewardProgramInfo;
        private bool updateAddressInfoAssociatedWithCC;
        private bool updateCreditCardInfo;
        private bool updateReservationPersist;
        private bool setEmailAsPrimay;
        private MOBCPTraveler traveler;
        private bool isTravelSavedToProfile = false;
        private List<int> alreadySelectedPAXIDs;
        private string cartId = string.Empty;
        private bool updateSpecialRequests;
        private bool updateServiceAnimals;
        private bool updateMealPreference;
        private bool clientCardType = false;
        private string flow;

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

        public bool UpdateTravelerBasiInfo
        {
            get { return this.updateTravelerBasiInfo; }
            set { this.updateTravelerBasiInfo = value; }
        }

        public bool UpdatePhoneInfo
        {
            get { return this.updatePhoneInfo; }
            set { this.updatePhoneInfo = value; }
        }

        public bool UpdateEmailInfo
        {
            get { return this.updateEmailInfo; }
            set { this.updateEmailInfo = value; }
        }

        public bool UpdateRewardProgramInfo
        {
            get { return this.updateRewardProgramInfo; }
            set { this.updateRewardProgramInfo = value; }
        }

        public MOBUpdateTravelerRequest()
            : base()
        {
        }

        public bool UpdateAddressInfoAssociatedWithCC
        {
            get { return this.updateAddressInfoAssociatedWithCC; }
            set { this.updateAddressInfoAssociatedWithCC = value; }
        }

        public bool UpdateCreditCardInfo
        {
            get { return this.updateCreditCardInfo; }
            set { this.updateCreditCardInfo = value; }
        }

        public bool UpdateReservationPersist
        {
            get { return this.updateReservationPersist; }
            set { this.updateReservationPersist = value; }
        }

        public bool SetEmailAsPrimay
        {
            get { return this.setEmailAsPrimay; }
            set { this.setEmailAsPrimay = value; }
        }

        public bool IsTravelSavedToProfile
        {
            get
            {
                return this.isTravelSavedToProfile;
            }
            set
            {
                this.isTravelSavedToProfile = value;
            }
        }
        public bool ClientCardType
        {
            get
            {
                return this.clientCardType;
            }
            set
            {
                this.clientCardType = value;
            }
        }
        public List<int> AlreadySelectedPAXIDs
        {
            get
            {
                return alreadySelectedPAXIDs;
            }
            set
            {
                this.alreadySelectedPAXIDs = value;
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

        public bool UpdateSpecialRequests
        {
            get { return updateSpecialRequests; }
            set { updateSpecialRequests = value; }
        }

        public bool UpdateServiceAnimals
        {
            get { return updateServiceAnimals; }
            set { updateServiceAnimals = value; }
        }

        public bool UpdateMealPreference
        {
            get { return updateMealPreference; }
            set { updateMealPreference = value; }
        }
        public string Flow
        {
            get { return flow   ; }
            set { flow = value; }
        }
    }
}
