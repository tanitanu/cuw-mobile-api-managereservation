using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.TeaserPage
{
    [Serializable]
    public class MOBSHOPShoppingTeaserPageRequest : MOBRequest
    {
        private string sessionID;
        private string cartID;

        public string SessionID
        {
            get
            {
                return sessionID;
            }
            set
            {
                sessionID = value;
            }
        }

        public string CartID
        {
            get
            {
                return cartID;
            }
            set
            {
                cartID = value;
            }
        }
    }
}

