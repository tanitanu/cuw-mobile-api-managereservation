using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable]
    public class MOBSHOPResponseStatusItem
    {
        private MOBSHOPResponseStatus status;
        public MOBSHOPResponseStatus Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        private List<MOBItem> statusMessages;
        public List<MOBItem> StatusMessages
        {
            get { return statusMessages; }
            set { statusMessages = value; }
        }
    }
}
