using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.UnfinishedBooking
{
    [Serializable]
    public class MOBSHOPUnfinishedBookingRequestBase : MOBRequest
    {
        #region Variables

        private string mileagePlusAccountNumber = string.Empty;
        private string passwordHash = string.Empty;
        private int customerId = -1;
        private int premierStatusLevel = -1;
        private bool isOmniCartSavedTrip;
        private string cartId;
        private string sessionId;
        #endregion

        #region Properties

        public string MileagePlusAccountNumber
        {
            get
            {
                return mileagePlusAccountNumber;
            }
            set
            {
                mileagePlusAccountNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string PasswordHash
        {
            get { return passwordHash; }
            set { passwordHash = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
        
        public int CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }

        public int PremierStatusLevel
        {
            get
            {
                return premierStatusLevel;
            }
            set
            {
                premierStatusLevel = value;
            }
        }

        #endregion
        private bool showOmniCartIndicator;

        public bool ShowOmniCartIndicator
        {
            get { return showOmniCartIndicator; }
            set { showOmniCartIndicator = value; }
        }
        public bool IsOmniCartSavedTrip
        {
            get { return isOmniCartSavedTrip; }
            set { isOmniCartSavedTrip = value; }
        }

        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }
        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }
        private bool isRemoveAll;

        public bool IsRemoveAll
        {
            get { return isRemoveAll; }
            set { isRemoveAll = value; }
        }
        private List<MOBItem> catalogItems;
        public List<MOBItem> CatalogItems
        {
            get { return catalogItems; }
            set { catalogItems = value; }
        }

    }
}
