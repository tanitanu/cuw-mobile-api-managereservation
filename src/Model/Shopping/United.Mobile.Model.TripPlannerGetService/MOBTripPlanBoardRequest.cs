using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.TripPlannerGetService
{
   public class MOBTripPlanBoardRequest: MOBRequest
    {
        private string sessionId = string.Empty;
        private string mpNumber = string.Empty;
        private bool isDeletedMessage;
        private bool isNotAvailableMessage;
        private string hashPinCode;

        public string HashPinCode
        {
            get
            {
                return hashPinCode;
            }
            set
            {
                this.hashPinCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }


        public string SessionId
        {
            set { sessionId = value; }
            get { return sessionId; }
        }       
        public string MpNumber
        {
            get { return mpNumber; }
            set { mpNumber = value; }
        }
        public bool IsDeletedMessage
        {
            get { return isDeletedMessage; }
            set { isDeletedMessage = value; }
        }
        public bool IsNotAvailableMessage
        {
            get { return isNotAvailableMessage; }
            set { isNotAvailableMessage = value; }
        }

    }
}
