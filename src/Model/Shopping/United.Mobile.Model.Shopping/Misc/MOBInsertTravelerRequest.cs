using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
//using United.Mobile.Model.Shopping.Booking;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class InsertTravelerRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string token = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private MOBCPTraveler traveler;
        private string cartId = string.Empty;
        private bool isTravelSavedToProfile = false;
        private List<int> alreadySelectedPAXIDs;
        private bool setEmailAsPrimay;

       
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
        public bool SetEmailAsPrimay
        {
            get { return this.setEmailAsPrimay; }
            set { this.setEmailAsPrimay = value; }
        }

        public InsertTravelerRequest()
            : base()
        {
        }
    }
}
